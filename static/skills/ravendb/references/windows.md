# RavenDB on Windows

Everything in `install.md` and `install-native.md` applies; the commands there are POSIX. Use the forms below on Windows, and read the rules — several are not a matter of translating a command.

## Probing for an existing server

```powershell
foreach ($u in 'http://localhost:8080','http://localhost:80') {
  try { $r = Invoke-WebRequest "$u/setup/alive" -UseBasicParsing -TimeoutSec 3; "$u $($r.StatusCode)" }
  catch { "$u unreachable" }
}
Get-Service RavenDB* -ErrorAction SilentlyContinue | Select-Object Name, Status
Get-Process Raven.Server -ErrorAction SilentlyContinue | Select-Object Id, Path
Get-NetTCPConnection -LocalPort 8080,80 -State Listen -ErrorAction SilentlyContinue |
  ForEach-Object { "{0} ← {1}" -f $_.LocalPort, (Get-Process -Id $_.OwningProcess).ProcessName }
docker ps -a --format '{{.Names}} {{.Image}} {{.Status}}' | Select-String raven
```

`Invoke-WebRequest` **throws** on any non-2xx, so a 404 from `/databases/<db>/stats` is an exception, not a status code — wrap probes in `try/catch` (PowerShell 7 also has `-SkipHttpErrorCheck`). In PowerShell 5.1 `curl` is an *alias* for `Invoke-WebRequest` and ignores POSIX curl flags; write `curl.exe` explicitly when you want the real thing (it ships with Windows 10 1803+).

## Docker Desktop

`ravendb/ravendb:latest` is a Linux image: it needs the WSL2 (or Hyper-V) backend, which is the default. Ports published from a Linux container reach `http://localhost:8080` on the Windows host normally, so the compose file in `install.md` works unchanged — write it with LF endings and let Docker mount named volumes rather than host paths (a bind-mounted Windows directory gives the container's non-root `ravendb` user permission errors on the data path).

Windows containers are a different image family — `ravendb/ravendb:<version>-windows-ltsc2022` or `-windows-1809`. Only reach for those if the Docker daemon is in Windows-container mode; the Linux image cannot run there and the tag will look mysteriously absent.

## Native install

```powershell
Invoke-WebRequest -Uri "https://hibernatingrhinos.com/downloads/RavenDB%20for%20Windows%20x64/latest" -OutFile ravendb.zip
Expand-Archive -Path ravendb.zip -DestinationPath C:\RavenDB -Force ; Remove-Item ravendb.zip
& 'C:\RavenDB\Server\Raven.Server.exe' --Setup.Mode=None --ServerUrl=http://127.0.0.1:8080 --ServerUrl.Tcp=tcp://127.0.0.1:38888
```

The zip holds a single top-level `RavenDB\` directory, so extracting to `C:\RavenDB` yields `C:\RavenDB\RavenDB\Server\...` unless you extract to the parent or move the contents up. Always quote paths in `&`-invocation; anything under `C:\Program Files` breaks unquoted. Background it with `Start-Process -FilePath … -ArgumentList … -WindowStyle Hidden`.

## Run as a service

There is **no** `Raven.Server.exe` service flag. Two supported routes, both requiring an **elevated** PowerShell:

```powershell
.\setup-as-service.ps1                                    # ships in the package; prompts for port, ACLs the dir, registers
& '<package>\Server\rvn.exe' windows-service register --service-name RavenDB -- --Setup.Mode=None --ServerUrl=http://127.0.0.1:8080
Start-Service RavenDB                                      # rvn windows-service start|stop also work
Get-Service RavenDB
```

`rvn.exe` ships with the server binaries — look in the package's `Server\` directory. Default service name is `RavenDB`; `--service-user-name` / `--service-user-password` run it as a specific account, `--server-dir` points at a server directory other than the one holding `rvn.exe`, and arguments after `--` are passed through to the server. `rvn.exe windows-service unregister --service-name RavenDB` removes it.

## WSL2

- **Windows → WSL:** a server started inside WSL is reachable at `http://localhost:8080` from Windows (localhost forwarding is on by default).
- **WSL → Windows:** `localhost` does **not** reach the Windows host. Use the host IP — `ip route show default | awk '{print $3}'` — or turn on mirrored networking (`networkingMode=mirrored` in `%UserProfile%\.wslconfig`, Windows 11 22H2+), after which `localhost` works both ways.
- A server bound to `127.0.0.1` inside WSL is invisible to Windows regardless; bind `0.0.0.0` when it must be shared, and remember that widens `Security.UnsecuredAccessAllowed` beyond `Local` — only do it for a dev server on a trusted machine.
- Writing a `.sh` helper from Windows or a Windows-mounted path produces CRLF line endings, and a `#!/bin/bash\r` shebang fails with a confusing "No such file or directory". Strip them: `sed -i 's/\r$//' <file>`.

## Firewall

Localhost needs nothing. Another machine reaching a **secured** server needs the port opened, from an elevated shell:

```powershell
New-NetFirewallRule -DisplayName "RavenDB" -Direction Inbound -Port 8080 -Protocol TCP -Action Allow
```

## Paths

Native install: `DataDir` defaults to `Databases/{name}` **relative to the server directory**, so a database lands in `<package>\Server\Databases\<database-name>`, with settings at `<package>\Server\settings.json`. Pass `--DataDir=D:\RavenData` to move it — and do move it off the install directory before an upgrade replaces that tree. Docker (Linux image) uses `/var/lib/ravendb/data` and `/etc/ravendb` inside the container instead, as in `install.md`.
