mergeInto(LibraryManager.library, {

   zappar_sns_initialize: function(canvas, unityObject, onSavedFunc, onSharedFunc, onClosedFunc) {
       var canva = document.querySelector(UTF8ToString(canvas));
       if(typeof canva === 'undefined' || canva === null) {
           window.snsInitialized=false;
           console.log(UTF8ToString(canvas)+" not found in document");
           return false;
       }
       window.unityCanvas = canva;
       window.unitySNSObjectListener = UTF8ToString(unityObject);
       window.unitySNSOnSavedFunc = UTF8ToString(onSavedFunc);
       window.unitySNSOnSharedFunc = UTF8ToString(onSharedFunc);
       window.unitySNSOnClosedFunc = UTF8ToString(onClosedFunc);
       if (typeof ZapparSharing === 'undefined' || ZapparSharing === null) {
            var scr = document.createElement("script");
            scr.src="https://libs.zappar.com/zappar-sharing/1.1.3/zappar-sharing.min.js";
            scr.addEventListener('load', function() {
                console.log("Zappar-sharing version 1.1.3 added");
                window.snsInitialized = true;
            });
            document.body.appendChild(scr);
        }else{
            window.snsInitialized = true;
        }
        return true;
   },

   zappar_sns_is_initialized : function() {
       if(typeof window.snsInitialized === 'undefined') return false;
       return window.snsInitialized;
   },

   zappar_sns_jpg_snapshot : function(img, size, quality) {
       if(typeof window.snsInitialized === 'undefined' || window.snsInitialized === false) return;
        //window.snapUrl = window.unityCanvas.toDataURL('image/jpeg',quality);
        var binary = '';
        for (var i = 0; i < size; i++)
            binary += String.fromCharCode(HEAPU8[img + i]);
        window.snapUrl = 'data:image/jpeg;base64,' + btoa(binary);
   },

   zappar_sns_png_snapshot : function(img, size, quality) {
       if(typeof window.snsInitialized === 'undefined' || window.snsInitialized === false) return;
        //window.snapUrl = window.unityCanvas.toDataURL('image/png',quality);
        var binary = '';
        for (var i = 0; i < size; i++)
            binary += String.fromCharCode(HEAPU8[img + i]);
        window.snapUrl = 'data:image/png;base64,' + btoa(binary);
   },

   zappar_sns_open_snap_prompt : function() {
       if(typeof window.snsInitialized === 'undefined' || window.snsInitialized === false) return;
       ZapparSharing({
        data: window.snapUrl,
        onSave: () => {
            window.uarGameInstance.SendMessage(window.unitySNSObjectListener, window.unitySNSOnSavedFunc);
        },
        onShare: () => {
          window.uarGameInstance.SendMessage(window.unitySNSObjectListener, window.unitySNSOnSharedFunc);
        },
        onClose: () => {
          window.uarGameInstance.SendMessage(window.unitySNSObjectListener, window.unitySNSOnClosedFunc);
        },
       });
   },

   zappar_sns_open_video_prompt : function() {
       if(typeof window.snsInitialized === 'undefined' || window.snsInitialized === false) return;
       ZapparSharing({
        data: window.videoRecUrl,
        onSave: () => {
            window.uarGameInstance.SendMessage(window.unitySNSObjectListener, window.unitySNSOnSavedFunc);
        },
        onShare: () => {
          window.uarGameInstance.SendMessage(window.unitySNSObjectListener, window.unitySNSOnSharedFunc);
        },
        onClose: () => {
          window.uarGameInstance.SendMessage(window.unitySNSObjectListener, window.unitySNSOnClosedFunc);
        },
       });
   },
});