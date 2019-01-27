console.log('inject intro references')
var iframe = window.frames["main"].document;
var iFrameHead = iframe.getElementsByTagName("head")[0];
var mycssscript = document.createElement('link');
mycssscript.rel = 'stylesheet';
mycssscript.type = 'text/css';
mycssscript.href = '//cdnjs.cloudflare.com/ajax/libs/intro.js/2.9.3/introjs.min.css';
iFrameHead.appendChild(mycssscript);

var iFrameBody = iframe.getElementsByTagName("body")[0];
var myscript = document.createElement('script');
myscript.type = 'text/javascript';
myscript.src = '//cdnjs.cloudflare.com/ajax/libs/intro.js/2.9.3/intro.min.js';
iFrameBody.appendChild(myscript);

let fields = [
    { 'ctl00_phF_form_t0_edDocType': 'Utilities and Rent Bill should have the type \'Bill\'' },
    { 'ctl00_phF_form_t0_edVendorID_text': 'Can be filled in with \'V000000002\' (UtilitiesCo)' },
    { 'ctl00_phF_form_t0_edDocDate_text': 'Can be filled in with \'1/1/2018\'' },
    { 'ctl00_phF_form_t0_edFinPeriodID_text': 'Can be filled in with \'01-2018\'' },
    { 'ctl00_phF_form_t0_edInvoiceNbr': 'Can be filled in with \'01012018U \'' },
    { 'ctl00_phF_form_t0_edDocDesc': 'For example \'Rent and utilities bill\'' },
    // {'amount': 'Can be filled in with\'2200.00\''},
    { 'ctl00_phF_form_t0_chkHold': 'Should be Cleared (to make the document ready for release)' }
]

for (var i = 0; i < fields.length; i++) {
    setDataIntroAttrToParent(iframe, Object.keys(fields[i])[0], Object.values(fields[i])[0], i + 1)
}

var yourCustomJavaScriptCode = 'setTimeout(function () {introJs().start();}, 1000);';
var script = document.createElement('script');
var code = document.createTextNode('(function() {' + yourCustomJavaScriptCode + '})();');
script.appendChild(code);
iFrameBody.appendChild(script);

var yourCustomCssCode = '.introjs-helperNumberLayer { top: -20px !important; left: -8px !important; background: #007acc !important; }';
var script = document.createElement('style');
script.appendChild(document.createTextNode(yourCustomCssCode));
iFrameHead.appendChild(script);

function setDataIntroAttrToParent(container, id, text, orderId) {
    if (container.getElementById(id)) {
        var parentDiv = container.getElementById(id).closest('.cell-w');
        if (parentDiv) {
            parentDiv.setAttribute('data-intro', text);
            parentDiv.setAttribute('data-step', orderId);
        }
    }
}


