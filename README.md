# Invoices Manager (written in WPF C#   DOTNET3.1)

## Important Info!
The program may contain errors, if any are found, please report them, 
I always recommend to make a BackUp of the Data Folder or to do it 
via the BackUp function in the program.


## Application description:
Are you also tired of putting all your invoices (and other documents) 
in one folder and then having to search for them? <br/>
Now it has an end, with this you can import and manage them. 
It's simple, there are enough ways to filter your documents.


## How to Download:
Go to the "Releases" and download any version. (for security the latest one).  <br/>
Or [press here](https://github.com/Schecher1/InvoicesManager/releases/download/InvoicesManager-Vers-1.2.0.2/InvoicesManager_WindowsX86.zip) to download if you want the latest one (normal) <br/>
Or [press here](https://github.com/Schecher1/InvoicesManager/releases/download/InvoicesManager-Vers-1.2.0.2/InvoicesManager_WindowsX86_Standalone.zip) to download if you want the latest one (standalone) <br/>


## Features:
✔️ Clean and modern UI<br/>
✔️ User friendly / beginner friendly<br/>
✔️ BackUp function<br/>
✔️ Many filter methods<br/>
                                                                                                             

## Images:
### Main-Screen:                                                  
![Main-Screen](IMAGES/Version%201.1.3.0/MainScreen.png)

### Invoice-View-Screen:                                           
![Invoice-View-Screen](IMAGES/Version%201.2.0.0/InvoiceViewScreen.png)

### SaveAs-Screen:
-You can choose in which format the file should be saved (the name), or you can save it as you wish                   <br/>
![SaveAs-Screen](IMAGES/Version%201.1.3.1/InvoiceSaveAsScreen.png)

### Setting-Screen:                                         
![Setting-Screen](IMAGES/Version%201.2.0.2/SettingScreen.png)

### About-Screen:                                         
![About-Screen](IMAGES/Version%201.1.3.0/AboutScreen.png)


# CHANGELOG

## v1.2.1.0
- Auto Backup function added, at each program start a backup is made and stored in a user defined folder. (can be managed in the settings)
- Auto Backup also has versions, so you can set that only MAX 10 versions of the backups can be exited, and the 11 will be deleted automatically. (can be managed in the settings) 


## v1.2.0.1 (HotFix for v1.2.0.0)
- Edit and Delete did not update the states, this has been fixed.
- When adding an invoice the date was cleared, this was also fixed


## v1.2.0.0 (!!! VERSION 1.1.4.0 AND BELOW ARE NOT COMPATIBLE WITH VERSION 1.2.0.0 AND HIGHER !!!)
- The config file now has a version (Later for the Program Updater(not yet implemented))
- The Invoice Model (Input) has been extended to the following (for the later updates and functions):
  -Tags<br/>
  -Importance State<br/>
  -Money State<br/>
  -Paid State<br/>
  -Money Total<br/>
  -Capture Date<br/>
- When a document is opened, it is started in the temp folder and not in the main directory (of course, they are deleted directly).
- Your root directory is no longer statically bound to a path, it is now dynamic
- You can now change your path (where the invoices are stored)

## v1.1.4.0
- Add a Progress Bar when loading the data (Import & Export)
- Delete the TempFiles after the Export (were not deleted before and left in the temp folder)

## v1.1.3.4
- Code cleaned up, optimized and improved
- The width of the SaveAsWindow was increased

## v1.1.3.3
- The program now has an icon/logo
- The date of the exhibition is now by default today's date instead of nothing

## v1.1.3.2
- Now you can sort the date (from new to old or old to new)

## v1.1.3.1
- From SaveAs Window made the font size bigger so you can see everything better
- Font color was adjusted (Windows LightMode users still see everything a bit strange, will be fixed soon)

## v1.1.3.0
- Fixed a bug where a time was displayed at the date of the exhibition (12:00:00 AM)
- Fixed a DynamicResource in the InvoiceAddScreen

## v1.1.2.0
- About Window glitch fixed

## v1.1.1.0
- Removed a few typos

## v1.1.0.0
- The program is more dynamic now, you can now modify the configurations
- You can change your PDF browser
- The program has "English" as default language
- The program is now multi-language capable
- The program is now available in English
- The program is now available in German

## v1.0.2.0
- The known refresh and combox glitch has been fixed.

## v1.0.1.0
- You can now BackUp you Invoices
- You can now Restore your Invoices
- You can Refresh you Board, if the board is not updated (known bug)

## v1.0.0.0
- The program use ModernWPF as Design
- You have 5 filter types to choose 
- You can add, edit, remove and save as an invoice
- Program is ONLY in German (for now)
