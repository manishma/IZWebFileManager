# IZWebFileManager for CkEditor #
This project is forked from [IZWebFileManager Manishima](https://github.com/manishma/IZWebFileManager). It proposes some small changes to the original project and demostates how use IZWebFileManager with [CkEditor](https://github.com/ckeditor) and [Tiny MCE](http://www.tinymce.com/). 
## License ##
My code is delivered under the free software/open source GNU General Public License (commonly known as the "GPL"). Form more informaztion about original IZWebFileManager license model refer to [project page](http://www.izwebfilemanager.com/).
## Changes log ##
IZWebFileManager proposed changes:

1. **Default image in png format**. In IZWebFileManager you may define a folder containing icons used for buttons, generic folders and generic file. In version 2.8.1 only gif images was recognized. I have added png image support.
2. **Custom Thumnnail Manager**. I have added *CustomThumbnailHandler* property tha allow you to define a custom handler for images thumbnails.

Added features:

1. **Custom Upload Manager**. In CkEditor Sample a Custom File Manager is used (based upon popular FineUploader) that implement drag and drop functionality.
2. **HTML Editors interface**. File Browser is ready to use. You need only to configure your preferred editor for an custom file browser (specific instructions and samples are provided). Go to site for more informations.
3. **Custom toolbar button**. I added to File Manager toolbar a custom button the show images in a bootstrap modal.

## Notes ##
The project use various external resources:
 
- **jQuery** (version 1.11.1 is included)  
- **Bootstrap Framework** . A subset of the  css framework version 3.2.0.1 was generated (and compressed) from the less files (provided). You could easy customize the subset changing and recompiling /CkEditorSample/Content/FileBrowser/FileBrowser.less.
- **Javascript plugin**. All plugins are bundled and minimized in /FileManager/js/FileManager.min.js. See /FileManager/js/FileManager.js.bundle file for details. Bundling and compressing was handled using **Web Essentials** a popular free Visual Studio Extension.
