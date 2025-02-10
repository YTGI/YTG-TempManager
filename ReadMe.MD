![YTG Logo](http://ytgi.com/images/ytgi.png)

# YTG-TempManager
<p>Yasgar Technology Group Temp folder management application</p>
<p>.NET 9.0</p>
<p>Console application utilizing dependency injection</p>
</br>

## Description
<p>This application will allow you to execute this EXE on startup. I have also found the need to create a windows scheduler to run it at midnight, 
in case you work past midnight or just leave your PC running overnight.</p>
<p>The application will create a temp folder in your designated Source folder with today's date, i.e. 20241027. 
Then the lookback days are used to move that folder to a destination folder if it has any content. If it has no content, it just deletes it.</p>

## About this app
<p>I was shown this concept by a Database developer decades ago. Have a new folder every day that you store notes, scripts, graphics and anythihng else temporary 
that you are working on. Then, say you need a script that you know you worked on a few weeks ago, you can find it easily, especially if you remember the day 
you worked on it.</p>
<p>I also have created a habit of creating a notes.txt file in this folder. I then can use a GREP app to find information from even years ago if needed.</p>
<p>Recommend running at leaset the destination folder in a location that is synced to the cloud, say with OneDrive etc.</p>

## Concepts
<p>This app code shows how to use dependency injection in a console application, which I found somewhat of a challenge to figure out the first time around.</p>

## Deploying
<p>This is a stand along EXE that requires the appsettings.json to be with it for the appropriate settings</p>

### PowerShell
<p><b>NOTE:</b> Must have the full path or the service will not start.</p>
<p>sc.exe create "YTG Temp Manager Service" binpath="C:\temp\TempManager\YTG.TempManager.exe" </p>
<p>sc.exe delete "YTG Temp Manager Service"</p>

