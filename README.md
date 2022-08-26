# HAL - Home Assistant Listener

![icon](https://user-images.githubusercontent.com/3686234/186933998-cf292bf1-424b-43a9-b3c4-42d644a513ae.png)

## Disclaimer

This project was created for personal use, so:

- Use it as you want, but I do not take responsibility for any damage or compromise due to the use itself.
- Maintenance or upgrade is not guaranteed, it was created for personal use and for fun.
- Communication and/or assistance regarding this project is not guaranteed.

## WHY

I needed to be able to run commands from Home Assistant to my Windows PC. And I created this WPF software that allowed me this interaction.

## HOW

I created a WPF App with Visual Studio 2022 that uses the .NET Framework 4.7.2 which creates a "server" that listens for calls on the local IP of the Windows PC on a specific port. On Home Assistant it is possible to invoke a service that executes a CURL call to the PC that reacts according to the parameters.

## REQUIREMENTS

The software already allows the system to be configured with specific buttons, but requires that it be run with administrator rights.  


You can also run these commands on your own (totally optional):

- It is necessary to open the port 8080 on the Windows Firewall.

  Add the rule on Firewall:  
  `netsh advfirewall firewall add rule name="Home Assistant Listener" dir=in action=allow protocol=TCP localport=8080`
  
  Remove the rule from Firewall:  
  `netsh advfirewall firewall delete rule name="Home Assistant Listener"`
  
- It is necessary to reserve the url for incoming calls: http://*:8080/HAL/

  Add the url:  
  `netsh http add urlacl url=http://*:8080/HAL/ user=Administrator`
  
  Remove the url:  
  `netsh http delete urlacl url=http://*:8080/HAL/`
  
  
## INSTALLATION

### Windows APP

There is currently no installation package, so compile it yourself. If I have time I will create it :P

To allow the application to start with administrator rights without asking for confirmation, I created a windows task with the check mark "Run with the highest privileges"

### Home Assistant

Add to your `configuration.yaml` these simple rows:
   ```
   shell_command:
     hal: "curl -X POST -d '{{ data }}' {{ url }}"
   ```
Restart Home Assistant and if you want call some action on your PC you can call a service like this:
   ```
   service: shell_command.hal
   data:
     url: http://[YOUR_LOCAL_IP]:8080/HAL/monitor_internal
     data: "{}"
   ```
Where `monitor_internal` is your command configured on the windows app.

![screenshot_app](https://user-images.githubusercontent.com/3686234/186932786-e1f7a64a-3913-404b-aec5-9ca3358a5fe4.jpg)


