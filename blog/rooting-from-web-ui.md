### One-click Android Application

 The fix to common problems with the NVG510 and NVG589 is now available as a push-button $3 Android app called [NVG510 Fixer](https://play.google.com/store/apps/details?id=com.earlz.nvg510fixer). You can see more information at [this blog post](http://earlz.net/view/2013/08/03/2006/nvg510-fixer-an-android-application). If you don't think it's worth it though, (or want to do crazy technical things), just stick with the free instructions on this page

<p> 
</p>
<p> 
</p>

This guide has been tested to work with the following hardware and firmware:

* NVG510 9.0.6h2d30
* NVG510 9.0.6h2d21
* NVG510 9.0.6h048
* NVG589 (unknown)

It's very possible that other Netopia OS based modems are affected as well. There is a Netopia modem used in Switzerland that probably can be rooted with this. 


## How to root the modem

### Warning

WARNING: This is information on how to root your modem. Rooting is to take full control, like rooting your Android phone. It can possibly brick your modem if not used responsibly. USE AT YOUR OWN RISK

### Enabling a telnet backdoor and reaching root shell

1. View the modem's update page, which should be at [http://192.168.1.254/cgi-bin/update.ha](http://192.168.1.254/cgi-bin/update.ha)
2. Login if you haven't already.
3. Now you'll want to view the HTML source of the page.
4. Search for the term "nonce" in the HTML source. You should see something like this:

    input type="hidden" name="nonce" value="815a0aaa0000176012db85d7d7cac9b31e749a44b6551d02"

5. Hang on to that piece of text and now load my [control2 page](http://earlz.net/static/control2.html) page. 
6. Take the "value" of the nonce and put it into the text field labeled nonce on the page. `815a0aaa0000176012db85d7d7cac9b31e749a44b6551d02` would be what you put into it for this example.
7. Load the page up and push Save.
8. At this point, you might see different things, depending on your browser. You might get a message that the page couldn't be displayed, or you might see some text. If you see the text, make sure that it says "backdoor telnet enabled" or some such. 
9. Now, you will need to reboot your modem. You can do this by doing to http://192.168.1.254/cgi-bin/restarting.ha
10. Now you should be able to login to the modem with telnet **on port 28**. The username is `admin` and the password is your modem's "access code" that should be written on it.
11. Finally, you should see the shell.

 
To clarify, your telnet session should look like this:

<pre>
[earlz@EarlzZeta ~]$ telnet 192.168.1.254 28
Trying 192.168.1.254...
Connected to 192.168.1.254.
Escape character is '^]'.

login: admin
Password:

Terminal shell v1.0
Copyright (C) 2011 Motorola, Inc.  All rights reserved.
Motorola Netopia Model NVG510 Wireless-N ADSL AnnexA Ethernet Switch
Running Netopia SOC OS version 9.0.6 (build h2d30)
ADSL capable
(admin completed login: Admin account with read/write access.)

NOS/XXX> 
</pre>

From here, you can do a few different things. This shell is called `nsh`. If you want to get to a root shell, just type `!`. At that point, you can do `exit` to get back to `nsh`.
Also, if you prefer the shell described in the FCC manual (and used by AT&T techs), you can type `cshell` after getting to the root shell. 

### How does it work?

I've found a vulnerability in the WebUI of the NVG510 (and other modems) that allows me to execute any command as root. You can utilize this by sending it a specially crafted HTTP request.

So, I use this to download the shell 
script at http://earlz.net/static/backdoor.nvg510.sh and execute it on the modem. What the shell script does is it will install a new service into `inetd` so that it starts a telnet
shell, and I configure it using `pfs` to be persistent. Otherwise, it would go away after rebooting. 

### Uninstalling the backdoor

The backdoor installed should be fairly safe, being password protected, but if you're especially worried, then it can easily be uninstalled.
Just telnet into the modem, get to the root shell by using `!`, and then type:

    pfs -r /var/etc/inetd.d/telnet28

Note! **This backdoor will not be uninstalled with a factory reset or firmware update!**. Once you've installed it, it's there until you uninstall it! Again, there should be no risk in leaving it there,
it will not be internet accessible. But, it's easy to uninstall as well. 

### Solving common problems

To confirm you're at `nsh`, you should see a prompt like this:

    Axis/1234565678> 

**The nsh Console**

This console is fairly simple and easy to use, and breaks out everything that you can configure on the modem. But, it is **not** the console described in the FCC manual. 

This is the help text for the console, to help you understand: 

<pre>
Axis/124578433> help
help [command]                 : Get help.
history                        : Show command history.
get OBJ.ITEM                   : Get the value of OBJ.ITEM (ITEM is a
                                 parameter or status). ### Hint: run 'info
                                 OBJ.params' or 'info OBJ.status' to get a
                                 list of the OBJ's parameters and status.
set OBJ.ITEM VALUE             : Set the value of OBJ.ITEM to VALUE.
info INFO [ARGS ...]           : Get the INFO information (expert mode).
new OBJ [NAME]                 : Create an object with an (optional) name
                                 (requires an 'apply')
del OBJ                        : Delete an object (requires an 'apply')
aget OBJ.ITEM ATTR             : Get the OBJ.ITEM's ATTR attribute.
aset OBJ.ITEM ATTR VALUE       : Set the OBJ.ITEM's ATTR attribute to VALUE.
name OBJ [NAME]                : Get or set the OBJ's "name" (specify a new
                                 name to set it).
names [OBJ]                    : Recursively show all object names.
validate [OBJ]                 : Validate OBJ, or the entire database if no
                                 OBJ specified.
apply                          : Apply changes to the database (changes are
                                 NOT saved).
revert                         : Revert the database by discarding your
                                 changes.
save                           : Save the database (rewrites config.xml).
defaults                       : Reset the system back to the factory
                                 defaults (deletes config.xml).
dump [OBJ [LEVELS]]            : Dumps the OBJ's parameters, or the entire
                                 database. Use the optional LEVELS parameter
                                 to limit the depth of the database tree.
sdump [OBJ [LEVELS]]           : Dumps the OBJ's status, or the entire
                                 database.
tdump [TEMPLATE [LEVELS]]      : Dumps the template, or the entire SDB schema.
dirty [OBJ]                    : Displays which parameters are dirty.
run CMD [ARGS ...]             : Run the SDB's CMD command (expert mode
                                 only!).
event EVT [ARGS ...]           : Send the EVT (event number) to the SDB
                                 (expert mode only!).
console [on | off]             : Direct all log messages to this console.
                                 Without arguments, toggles on and off.
log [OPTIONS]                  : View log messages. See "log help" for more
                                 information.
voiplog [OPTIONS]              : View log messages. See "log help" for more
                                 information.
mfg [OPTIONS]                  : Set or view MFG parameters. See "mfg help"
                                 for more information.
mirror [PORT CAPTURE-PORT] | "off" : Mirror Ethernet traffic on PORT so that it
                                 may seen on CAPTURE-PORT. Specify "off" to
                                 turn mirroring off.
resetstats [OBJ] ["all"]       : Reset any statistics the object may have.
                                 The optional "all" argument will recursively
                                 reset all children's stats as well. If only
                                 "all" is given (OBJ is omitted), this will
                                 reset all statistics starting at the root
                                 node.
metadata OBJ.PARAM             : Returns metadata information about a given
                                 parameter.
fwinstall URL | "last"         : Install a firmware image. Use "last" to
                                 reuse the last URL.
crashdump ["erase"]            : Shows the most recent crash dump contents.
                                 The optional "erase" will erase both current
                                 and last saved crash dump contents.
reboot [N] | ["cancel"]        : Reboot the router in N seconds (default is
                                 2). "cancel" argument can be issued to
                                 cancel a previous reboot command.
source FILE                    : Read and process commands from FILE.
. FILE                         : An alias for 'source'.
exit                           : Exit from this shell.
quit                           : An alias for 'exit'.
magic                          : Enter magic mode.
crash                          : Read and Write the Memory mapped registers
</pre>

Seems simple enough then doesn't it? 

**Example Configuration**

So, let's say you want to enable SSH. The relevant configuration option for this is `mgmt.shell.ssh-port`. So, to set this, we type this in:

    set mgmt.shell.ssh-port 22

This will set the SSH port to 22, rather than disabled. And then, if you're done configuring, you can save and apply the changes by typing these commands in:

    validate
    apply
    save

You don't necessarily have to do validate, but I assume it's safer to use it I think. I believe that this is what happens:

1. `validate` will validate the changes to make sure that no data was input in a way that wouldn't make sense (like if nameserver was set to `921.123.45.673`)
2. `apply` will actually cause the modem to notice the changes and begin executing using those changes you've made
3. `save` will cause the changes you made to persist after reboot. I assume it saves it to flash with this command. 

That's really about all there is to know. Configuration is super simple. 

**Configuration**

As you can tell from the dump log, there are a ton of configuration options. Here I'll give you a hint to the more useful ones, as well as some configuration stuff to be aware of

**DNS problem fix**

This is provided for historical reasons, but it's **WRONG**. This will not fix the DNS problems or let you point it at a different DNS server. I don't know why it doesn't work, but I've received multiple reports that it doesn't. Your best bet in this case is to use the true bridge mode and get your own router
 
    ip.dns.domain-name             = att.net
    ip.dns.primary-address         = 99.99.99.53
    ip.dns.secondary-address       = 99.99.99.153
    ip.dns.proxy-enable            = on
    ip.dns.override-allowed        = off

You should be able to change these to something more appropriate. `override-allowed` should be turned on(otherwise I believe they will be reset by DHCP over the DSL link). 

**Enabling Telnet and/or SSH**

    mgmt.shell.ssh-port            = 0
    mgmt.shell.telnet-port         = 0

These you should change to what port you want it to run on. Note though that I've yet to figure out the username and password used for SSH. I've searched through both the dump and through the GPL source code and can't find any hints really. 

So, to enable these you can just do something like

    set mgmt.shell.ssh-port 22
    set mgmt.shell.telnet-port 23
    validate
    apply
    save

If you want to enable remote access to telnet and/or ssh (I highly recommend not opening up telnet to the world) you can modify these values to something appropriate:

    mgmt.remoteaccess[3].protocol  = telnet
    mgmt.remoteaccess[3].port      = 0    XX change this to 23
    mgmt.remoteaccess[3].idle-timeout = 5
    mgmt.remoteaccess[3].total-timeout = 20
    mgmt.remoteaccess[3].max-clients = 4
    mgmt.remoteaccess[4].protocol  = ssh
    mgmt.remoteaccess[4].port      = 0     XX change this to 22
    mgmt.remoteaccess[4].idle-timeout = 5
    mgmt.remoteaccess[4].total-timeout = 20
    mgmt.remoteaccess[4].max-clients = 4

**Enabling UPnP**

I haven't confirmed this, but I believe UPnP can be enabled by changing this to on:

    mgmt.upnp.enable               = off

**Disable "Potential Connection Issue" warnings**

    mgmt.lan-redirect.enable       = on

Change it to `off`. lan-redirect is what causes that extremely annoying redirecting to happen when the connection is lost or "has possible problems". What the modem will do is when you request a nameserver, it will, instead of sending back no route, timeout, or the actual name servers response, 
it will instead make every domain forward to 192.168.1.254, so that you can then load an HTML page that causes a redirect(but doesn't set it to do-not-cache) to `/cgi-bin/home.ha`... So basically, you click `do not show`, yet the page continues to try to redirect
due to modern web browser caching and the lack of a no-cache directive on the redirect page. 

**Disabling the DHCP server**

    conn[1].dhcps-enable           = on

Note that you'll have to configure a static IP to the modem to access it after this. I don't see much of a point in disabling it completely.

**True Bridge Mode**

A very often wanted feature of the NVG510 is for it to just get out of your way and let your (hopefully more sane) router to deal with all the firewall and NAT business. After quite a bit of experimenting and starting over with `default` and a bit of an accident, I believe I've figured it out.

Some of the values in the NVG510's configuration "database" appears to be magical, and lots of assumptions have to be made without real technical documentation. So, let's look at the `link` object that appears to be linked to WAN and LAN `connections` in an assumed manner. 

Here is what was in my modem's dump about `link`s. Your's should look similar:
<pre>
link[1].type                   = ethernet
link[1].igmp-snooping          = off
link[1].mtu-override           = 0
link[1].port-vlan.ports        = lan-1 lan-2 lan-3 lan-4 ssid-1 ssid-2 ssid-3 ssid-4
link[1].port-vlan.priority     = 0
link[2].type                   = ethernet
link[2].mtu-override           = 0
link[2].supplicant.type        = eap-tls
link[2].supplicant.qos-marker  = AF1
link[2].supplicant.priority    = 0
link[2].port-vlan.ports        = vc-1
link[2].port-vlan.priority     = 0
link[2].tagged-vlan[1].ports   = ptm
link[2].tagged-vlan[1].vid     = 0
link[2].tagged-vlan[1].priority = 0
</pre>

`ptm` is the PPP connection. So we basically want for the PPP connection to be routed straight to an ethernet port so our router can handle it. So here is what I did

    set link[1].port-vlan.ports "lan-2 lan-3 lan-4"
    set link[2].port-vlan.ports lan-1

The first command sets the `LAN` link so that only the LAN ports 2-4 is used. The next link sets the link for the `WAN` side of the link. Previously, the port is vc-1. I assume vc-1 is hardwired to magically go to the `LAN` somehow. Anyway, replacing vc-1 with `lan-1` basically makes the equivalent of a PPP bridge. 

On the router side, all you have to do is use that port and the modem will do all of the PPP authentication, and I assume MRU shifting to 1500.. All your modem will get is a raw stream from AT&T's servers. So if you send it a DHCP client request, you'll get a response straight from AT&T's servers. 

This is the only configuration required as well. This will short through all of the modem's crappy configuration and directly forward  it to the first ethernet port(the one closest to the barrel jack power adapter).

And if for some odd reason you need to access the actual modem(such as for reconfiguration), just plug your network cable into another port. The built-in DHCP server runs just as before, except it will never be connected to the internet.

Note: I've had reports that this doesn't completely work when your account is provisioned with multiple static IP addresses. If you have problems and are willing to lend me some time to test things with you, email me at earlz -AT- earlz dot net

**Possible Problem**: If your modem seems to "hang" when doing `apply` with the bridge mode configuration and you can't use the `save` command, then that means you tried to do it *from* port-1. Change which port on the NVG510 your computer is plugged into(or use Wifi if you're extra brave)


**Other Dangerous Things**

From this bootloader, you can change a lot of things AT&T probably would frown upon. Basically, you can make it look like another modem. I'm not for sure about this though and will have to test it and research it more. I don't recommend changing anything in the `mfg` section. If you do one of these kind of hacks, be prepared for AT&T to notice and ban you from U-Verse, your modem to become bricked, or for your dog to randomly die. Don't go too far into the dangerous looking unknown. 

**Conclusion**

The NVG510 is really a decent modem, but has been kiddie-proofed so hard that it hurts. I hope this guide helps you to taking full control of your modem. Also, I don't recommend trying to evade your U-Verse accounts capabilities. 
I imagine AT&T won't care much if they catch you modifying your modem... they will care if you modified it to reach 16Mbit speeds when you only have a 3Mbit account though, and I'm sure they keep tabs on it. Don't be stupid. 

Same goes for trying to boost wifi power or use channels not specified for use in your country. The FCC is real! (btw, don't tell them about my FM transmitter project ;) )

**Configuration Template**
You can dump this for yourself, but to see what Motorola's "template" is for it's configuration options you can check out this [pastebin](http://pastebin.com/rw1ZrH5C). If you don't know what options a configuration object supports, this is a good bit to look at. Though a few things in the template don't exist in my NVG510 at least and will cause crashes if objects are created. (cifs will not work for me) 


### Older Versions

This is a new hack that should work on old firmware. However, if you're interested in the old hack(that only works on old firmware), you can see the [wayback machine](http://web.archive.org/web/20130822070025/http://earlz.net/view/2012/06/07/0026/rooting-the-nvg510-from-the-webui) for a historical copy. 
