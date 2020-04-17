::---------------------------------
RD /S /Q "bin\Release"
dotnet publish -c Release
IF EXIST "deploy" ( RD /S /Q "deploy" )
::---------------------------------
mkdir deploy
xcopy "bin\Release\netcoreapp3.0\publish\*" "deploy\"
::---------------------------------
robocopy "wwwroot" "deploy/wwwroot" /mir
robocopy "resources" "deploy/resources" /mir
::---------------------------------
echo "Enter pass"
bash -c "ssh root@37.148.210.36 'systemctl stop kestrel-mysite4.service'"
bash -c "rsync -r -v -c -h --progress --perms --chmod=a+rwx deploy/ root@37.148.210.36:/var/www/mysite4 --exclude=database --delete"
bash -c "ssh root@37.148.210.36 'systemctl start kestrel-mysite4.service'"
RD /S /Q "deploy"