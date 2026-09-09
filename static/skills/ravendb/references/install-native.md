# Native installation

No Docker: download the server archive, extract, run the binary. Health check, database creation, and licensing are identical to the Docker path — see `install.md`. Commands here are POSIX; on Windows read `windows.md` instead.

## Download

`/latest` redirects to the current stable build, so follow redirects (`curl -L`; `Invoke-WebRequest` already does).

| Platform | URL under `https://hibernatingrhinos.com/downloads/` | Archive |
|---|---|---|
| Windows x64 | `RavenDB%20for%20Windows%20x64/latest` | `.zip` |
| Linux x64 | `RavenDB%20for%20Linux%20x64/latest` | `.tar.bz2` |
| Linux ARM64 | `RavenDB%20for%20Linux%20ARM64/latest` | `.tar.bz2` |
| Ubuntu 24.04 x64 | `RavenDB%20for%20Ubuntu%2024.04%20x64%20DEB/latest` | `.deb` |
| macOS Intel (`uname -m` = `x86_64`) | `RavenDB%20for%20macOS%20x64/latest` | `.tar.bz2` |
| macOS Apple Silicon (`uname -m` = `arm64`) | `RavenDB%20for%20macOS%20arm64/latest` | `.tar.bz2` |
| Raspberry Pi | `RavenDB%20for%20Raspberry%20Pi/latest` | `.tar.bz2` |

Labels are case- and word-exact — `macOS%20ARM` or an Ubuntu label without `%20DEB` returns 404. When one does, read the current list off <https://ravendb.net/download>. Other Ubuntu releases (18.04 / 20.04 / 22.04) and arm32/arm64 DEBs follow the same pattern.

```bash
curl -L --progress-bar -o ravendb.tar.bz2 "https://hibernatingrhinos.com/downloads/RavenDB%20for%20Linux%20x64/latest"
mkdir -p ~/ravendb && tar xjf ravendb.tar.bz2 -C ~/ravendb --strip-components=1 && rm ravendb.tar.bz2
```

Windows download, extraction, and service registration: `windows.md`.

Everything sits under a single top-level `RavenDB/` directory in the archive — hence `--strip-components=1` above — with the binary at `Server/Raven.Server` (`Server\Raven.Server.exe` on Windows) and `run.sh` / `install-daemon.sh` next to it. Inside a project, `./support/RavenDB` (gitignored) keeps the install with the code; otherwise `~/ravendb` or `C:\RavenDB`.

## Run (development, unsecured)

```bash
~/ravendb/Server/Raven.Server --Setup.Mode=None \
  --ServerUrl=http://127.0.0.1:8080 --ServerUrl.Tcp=tcp://127.0.0.1:38888 \
  --DataDir=/srv/ravendata
```

Both ports are picked here rather than remapped as in Docker, and both must be free — substitute any pair (`8099` / `38899`) when another server already holds them, and report the HTTP one you settled on. `--DataDir` decides where databases land before the first one is created; adding it later leaves the earlier tree behind and the server comes up empty.

No unsecured-access flag is needed for a loopback bind: `Security.UnsecuredAccessAllowed` defaults to `Local`, which is exactly `127.0.0.1`. Widening it (`PrivateNetwork`, `PublicNetwork`) is only for a server that must answer on a LAN or routable address — and an unsecured server should not. Background it with `nohup … &`. Then health-check and create the database as in `install.md`.

## Run as a service

- **Windows:** `rvn.exe windows-service register` from an elevated shell — there is no `Raven.Server.exe` service flag. Commands, service naming, and the rest of the Windows specifics: `windows.md`.
- **Linux:** a systemd unit; `sudo` is needed for the install steps, so print them for the user rather than running them.

```ini
[Unit]
Description=RavenDB
After=network.target

[Service]
Type=simple
ExecStart=/home/USER/ravendb/Server/Raven.Server --Setup.Mode=None --ServerUrl=http://127.0.0.1:8080 --ServerUrl.Tcp=tcp://127.0.0.1:38888
WorkingDirectory=/home/USER/ravendb/Server
Restart=on-failure
RestartSec=10
LimitNOFILE=65536
User=USER

[Install]
WantedBy=multi-user.target
```

```bash
sudo cp ravendb.service /etc/systemd/system/ && sudo systemctl daemon-reload && sudo systemctl enable --now ravendb
```

`LimitNOFILE=65536` matters: RavenDB memory-maps its data files and hits the default 1024 descriptor cap under load.

## Stopping / cleanup

`systemctl stop ravendb`, or kill the process on the port (`kill $(lsof -t -i:8080)`). Data lives where `DataDir` points; without it, `Databases/{database-name}` **relative to the server directory** — `~/ravendb/Server/Databases/<name>`, with settings at `Server/settings.json`. Deleting that tree resets every database, and an upgrade replaces the install directory, so keep the data outside it.

## Reaching it from another machine

Only for a secured server. A firewall usually needs the port opened: `sudo ufw allow 8080/tcp`, or `sudo firewall-cmd --add-port=8080/tcp --permanent && sudo firewall-cmd --reload`.
