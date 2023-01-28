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
Or [press here](https://github.com/Invoices-Manager/Invoices-Manager-Windows/releases/download/InvoicesManager-Vers-1.3.2.0/InvoicesManager_WindowsX86.zip) to download if you want the latest one (normal) <br/>
Or [press here](https://github.com/Invoices-Manager/Invoices-Manager-Windows/releases/download/InvoicesManager-Vers-1.3.2.0/InvoicesManager_WindowsX86_Standalone.zip) to download if you want the latest one (standalone) <br/>


## Features:
✔️ Clean UI<br/>
✔️ User friendly / beginner friendly<br/>
✔️ BackUp function<br/>
✔️ Built in notebook<br/>
✔️ Auto BackUp function<br/>
✔️ Many filter methods<br/>
✔️ File database is optimal so that other programs can also use it<br/>
                                                                                                             

## Images:
### Main-Screen:                                                  
![Main-Screen](IMAGES/Version%201.4.0.0/MainScreen.png)

### Invoice-Main-Screen:                                                  
![Invoice-Main-Screen](IMAGES/Version%201.4.0.0/InvoiceMainScreen.png)

### Invoice-View-Screen:                                           
![Invoice-View-Screen](IMAGES/Version%201.4.0.0/InvoiceViewScreen.png)

### SaveAs-Screen:
-You can choose in which format the file should be saved (the name), or you can save it as you wish                   <br/>
![SaveAs-Screen](IMAGES/Version%201.4.0.0/InvoiceSaveAsScreen.png)

### Notebook-Screen:                                         
![Notescreen-Screen](IMAGES/Version%201.3.0.0/NotebookScreen.png) 

### Setting-Screen:                                         
![Setting-Screen](IMAGES/Version%201.4.0.0/SettingScreen.png) 

### About-Screen:                                         
![About-Screen](IMAGES/Version%201.4.0.0/AboutScreen.png)


# CHANGELOG
## Version structure (X.Y.Z.W)
### X = Major version
### Y = Minor version (big updates)
### Z = Minor version (small updates)
### W = Revision version (bug fixes)

## v1.4.0.0
- Switched from .NET 3.1 to **.NET 6.0** (when you start the program, you will be asked if you want to install .NET 6.0, if you don't want to install it, you can also download the standalone version)
- User interface has been improved and made more user-friendly and clear.
- A UI error was fixed in the "Money" column

## v1.3.2.0
- A log function was added, everything will be logged, from infos to errors
 
## v1.3.1.0
- !! The Config file was moved into the Data folder !! **(that means you have to move your Conifg.json file which is where the exe is located to the corresponding "data" folder)**
- The program optimized so that it needs less ram
- The program can now be started only once

## v1.3.0.0
- There is now a main menu where you can switch to certain interfaces
- When the main menu gets closed, all other programs get closed as well
- You can now double-click on a cell to store its content in your clipboard
- The Notebook is now also included in the BackUp. (its works like the Invoice BackUp, see "v1.2.3.0")
- The BackUp system also works with older BackUp files (up to v1.2.0.0)
- The Capture Date now also includes the time, which was previously formatted as 00:00:00
- The DarkMode was replaced by a LightMode
- Invoices Manager no longer uses "ModernWPF" (for later developments with own design)
- The Organization field can now also just be left blank (it is no longer a required field).
- The other data from the invoice are now also displayed in the DataGrid
- You have now 5 new filter types to choose (Tags, MoneyTotal, ImportanceState, MoneyState & PaidState) 
- DataGrid columns can be hidden by your selection (right-click on the DataGrid).


## v1.2.3.2
- SaveAs had a logical error, you could not select/use a custom filename

## v1.2.3.1 (HotFix for v1.2.3.0)
- Message box added that warns you that the notebook is not included in the backup. (Pops Up when you click on the Backup button)
- Program wont start, program needs earlier the conf file than it can be created

## v1.2.3.0
- There is now a notebook available, the path can also be changed in the config (!!! The notebook is not included in BackUp !!!)
- If a BackUp will be restored, then it is NOW also looked whether the invoice data has changed, if so then he overwrites the "old" invoice!
- Config file ist now human-readable

## v1.2.2.1
- Old icons replaced with the new ones that have been forgotten
- Invoices limited to *.pdf files only (for now)

## v1.2.2.0
- The invoice number can now also just be left blank (it is no longer a required field).
- FIX: Program "crashed" (the backup task) during BackUp when refreshing the list. 
- Dialogs have been added to settings so that you no longer have to awkwardly insert the path

## v1.2.1.1 (HotFix for v1.2.1.0)
- The autobackup stopped the whole program until it was finished (window did not show up) The tasks have now been moved to the other thread so that the program can start directly and does not have to wait

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
