Linux Debugging
============

RavenDB Linux debugging of core dump and running process can be done using the SOS library, with similar way as being done with WinDbg tool on windows. The tool for debugging is `lldb` and the dotnet’s main page describing the process is available here: https://github.com/dotnet/coreclr/blob/master/Documentation/building/debugging-instructions.md

To date (with coreclr v2.2.2 and Ubuntu-18.04) the following instructions can achieve attached debugger to a running process. Note that all operations should be done directly on the running machine:

Prerequsites:
```
sudo apt-get install -y python2.7 cmake clang-3.9 libicu-dev liblldb-3.9-dev liblttng-ust-dev libssl-dev libkrb5-dev libcurl4-openssl-dev lldb-3.9 libssl1.0 git make libc6-dev zlib1g-dev libunwind8 curl
 libssl1.0-dev

```
<version> is deteremined by : 
```
cat /path/to/server/runtime.txt
```
and the code for compiling coreclr and corefx:
```
WORKDIR=${HOME} # or your preferred dir
cd ${WORKDIR}/
git clone https://github.com/dotnet/corefx
cd corefx
git checkout tags/<version>
./build.sh -release
cd ${WORKDIR}/
git clone https://github.com/dotnet/coreclr
cd coreclr
git checkout tags/<version>
./build.sh -release
cp -a ${WORKDIR}/corefx/bin/ref/* ${WORKDIR}/coreclr/bin/Product/Linux.x64.Release/
```

Once attach to the running RavenDB process:
```
cd ${WORKDIR}/coreclr/bin/Product/Linux.x64.Release
sudo lldb-3.9 -p $(pgrep Raven.Server) -o "plugin load libsosplugin.so" -o “process handle -s false -p false -n false SIGPIPE SIG34" -o "continue"
```

Usefull lldb commands:
* Print all stacks - `eestack`
* Print all threads - `sos Threads`
* Select specific thread - `thread select <num>`
* Dump heap - `eeheap`
* Print selected thread - `clrstack -f`
* Disassmeble & info registers - `disassmble` , `register read`

Usefull commands mapping to `gdb` users: https://lldb.llvm.org/lldb-gdb.html

Cross-Building for other platforms can be achived using docker instances. For example, the following script will do the job for centos 7:
```
# centos 7:
PLAT=microsoft/dotnet-buildtools-prereqs:centos-7-d485f41-20173404063424 

WORKDIR=${HOME}/ravendbDebugRepos
CLRVER=2.1.8

[ -d ${WORKDIR} ] || mkdir ${WORKDIR}
pushd ${WORKDIR}

REPO=corefx
git clone https://github.com/dotnet/${REPO} ${REPO}-${CLRVER}
[ $? -eq 0 ] || (echo "ERROR: Failed to clone  ${REPO}" && popd && exit 1)
pushd ${REPO}-${CLRVER}
git checkout tags/v${CLRVER}
[ $? -eq 0 ] || echo "ERROR: Failed to checkout tag v${CLRVER} for ${REPO}" && popd && popd && exit 1
./build-managed.sh
popd

REPO=coreclr
git clone https://github.com/dotnet/${REPO} ${REPO}-${CLRVER}
[ $? -eq 0 ] || (echo "ERROR: Failed to clone  ${REPO}" && popd && exit 1)
pushd ${REPO}-${CLRVER}
git checkout tags/v${CLRVER}
[ $? -eq 0 ] || echo "ERROR: Failed to checkout tag v${CLRVER} for ${REPO}" && popd && popd && exit 1
sudo docker run --rm -v ${PWD}:/coreclr -w /coreclr ${PLAT} ./build.sh  -release
popd

cp -a corefx/bin/ref/* coreclr/bin/Product/Linux.x64.Release/
popd
```

On arm32, lldb (to date) provides false stacktraces, so `gdb` is the only way to debug RavenDB without manage code stack frames. Here is how to do that with `gdb`:
```
sudo gdb ./Raven.Server 
(gdb) handle SIG34 SIGPIPE nostop noprint pass
(gdb) run
```



