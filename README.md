MobileTasks
------
MobileTasks is an app developed by Brent Edwards and Kevin Ford as a demo for our VSLive! workshop, Building Modern Mobile Apps.

This repo contains all our demo code and slide decks. The code is expecting some configuration info which is specific to our Azure account. We have left our details out and added example files, where needed, for you to use your own info.

Here are the changes you'll need to do if you want to run the code for yourself:

**Server Code**
Source/Server/MobileTasks.Server.Api/ConnectionStrings.private.example
* Rename the file to remove the .example file extention, leaving ConnectionStrings.private.
* Add your connection string to your data source.

**Client Code**
* Windows|Xamarin.Android|Xamarin.iOS|Xamarin.Forms all have a Services folder with a file: MobileService.Private.Example.cs. Remove .Example from the file name and change the urls to match your Azure account.