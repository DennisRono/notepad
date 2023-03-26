const { BrowserWindow, app, Menu, ipcMain, dialog } = require("electron");
const fs = require("fs-extra");
const { FileManager } = require("./lib/FileManager");

const path = require('path');

app.on("ready", build_app);

async function build_app() {
  const app_window = new BrowserWindow({
    webPreferences: {
      nodeIntegration: true,
    },
  });

  app_window.loadFile("asset/index.html");

  const fileManager = new FileManager(app_window);

  // set open recent submenu
  let submenuOfOpenRecent = [];
  let paths = fileManager.readHistory();
  const allPaths = await paths;
  if (allPaths !== undefined) {
      allPaths.paths.map((path) => {
          submenuOfOpenRecent.push({ label: path, click: function () { fileManager.openRecentFile(path) } }, { type: 'separator' });
      })
  }

  function focusAndPerform(methodName) {
    return function(menuItem, window) {
      window.webContents.focus()
      window.webContents[methodName]()
    }
  }

  // Declare all menu
  let menu_list = [
    {
      label: "File",
      submenu: [
        {
          label: "Open File...",
          click: function () {
            fileManager.openFileWindow();
          },
        },
        {
          label: "Open recent...",
          submenu: submenuOfOpenRecent,
        },
      ],
    },
    {
      label: 'Edit',
      submenu: [
        {
          label: "Undo Ctrl + Z",
          click: focusAndPerform("undo"),
        },
        {
          label: "Redo Ctrl + Shift + Z",
          click: focusAndPerform("redo"),
        },
        { type: 'separator' },
        {
          label: "Cut Ctrl + X",
          click: focusAndPerform("cut"),
          enabled: false
        },
        {
          label: "Copy Ctrl + C",
          click: focusAndPerform("copy"),
          enabled: false
        },
        {
          label: "Paste Ctrl + V",
          click: focusAndPerform("paste"),
        },
        { type: 'separator' },
        {
          label: "Delete Del",
          click: focusAndPerform("delete"),
        },
        { type: 'separator' },
        {
          label: "Find Del",
          click: function () {
            
          },
        },
        {
          label: "Find next Del",
          click: function () {
            
          },
        },
        {
          label: "Find previous Del",
          click: function () {
            
          },
        },
        {
          label: "Replace Del",
          click: function () {
            
          },
        },
        {
          label: "Go To Del",
          click: function () {
            
          },
        },
        {
          label: "Select all Del",
          click: function () {
            
          },
        },
        { type: 'separator' },
        {
          label: "Time/Date Del",
          click: function () {
            
          },
        },
        {
          label: "Font Del",
          click: function () {
            
          },
        },
      ],
    },
    {
      label: 'View',
      submenu: [
        { role: 'reload' },
        { role: 'forceReload' },
        { role: 'toggleDevTools' },
        { type: 'separator' },
        { role: 'resetZoom' },
        { role: 'zoomIn' },
        { role: 'zoomOut' },
        { type: 'separator' },
        { role: 'togglefullscreen' }
      ]
    },
  ];

  // set the menu to desktop app
  const menu_design = Menu.buildFromTemplate(menu_list);
  Menu.setApplicationMenu(menu_design);

  // recieve new file data and path throught main and renderer method
  ipcMain.on("newdata", (e, arg) => {
    console.log(arg);
    if(arg.path!==''){
      fs.writeFile(arg.path, arg.file, (err) => {
        if (err) {
          throw err;
        }
        fileManager.saveHistory(arg.path)
        console.log("data saved");
      });      
    } else {
      // Resolves to a Promise<Object>
      dialog.showSaveDialog({
          title: 'Select the File Path to save',
          defaultPath: path.join(__dirname, '../assets/New Note.txt'),
          buttonLabel: 'Save',
          // Restricting the user to only Text Files.
          filters: [
              {
                  name: 'Text Files',
                  extensions: ['txt', 'docx']
              }, ],
          properties: []
      }).then(file => {
          // Stating whether dialog operation was cancelled or not.
          console.log(file.canceled);
          if (!file.canceled) {
              console.log(file.filePath.toString());
              // Creating and Writing to the sample.txt file
              fs.writeFile(file.filePath.toString(), arg.file, (err) => {
                if (err) {
                  throw err;
                }
                fileManager.saveHistory(file.filePath.toString())
                console.log("data saved");
              });
          }
      }).catch(err => {
          console.log(err)
      });
    }
  });
}
