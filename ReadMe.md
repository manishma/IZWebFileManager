# IZWebFileManager for CkEditor #
This project proposes some small changes to [IZWebFileManager by Manishima](https://github.com/manishma/IZWebFileManager). I've used IZWebFileManager to privide file browser features to [CkEditor](https://github.com/ckeditor), [Tiny MCE](http://www.tinymce.com/) or to your own application. This little project named [MB FileBrowser](https://github.com/magicbruno/FileBrowser) is mantained in a separate repository. If you are looking for a ready-to-use, intallable via Nuget, solution refer to [MB FileBrowser](https://github.com/magicbruno/FileBrowser) repository or [demo site](http://filemanager.sisteminterattivi.org/). 

## Changes ##

**Default image in png format**. In IZWebFileManager you may define a folder containing icons used for buttons, generic folders and generic file. In version 2.8.1 only gif images was recognized. I have added png image support.

Added properties:

- **MainDirectory**: It's the path of root directory for folder navigation (used in MB FileBrowser). Internet Anonimous User MUST have full control (read, write, delete/overwrite) over this folder. At the first run,  MB FileBrowser create automatically four subdir (*files*, *images*, *flash* and *media*) where you may organize user files. Folders are created to accomplish the rules that both CkEditor that Tiny MCE follow in file organization.

- **CustomThumbnailHandler**: Url of the ThumbnailHandler. IZWebFileManager provides a default thumbnail handler (IZWebFileManagerThumbnailHandler.ashx), but you may write your own. IZWebFileManager call the Thumbnail Handler passing url-ecoded image file url as querystring (ex: MyThumbnailHandler.ashx?%2DirName%2imagename.jpg).  

- **DefaultAccessMode**: (used in MB FileBrowser). It is highly recommended to grant access to the server folders only to registered users. Probably you will define access mode during  the process of authenticating users (see [MB FileBrowser](https://github.com/magicbruno/FileBrowser)). This property determines what kind of access is granted to guest users (or when session expires). Default value is *ReadOnly*. 
