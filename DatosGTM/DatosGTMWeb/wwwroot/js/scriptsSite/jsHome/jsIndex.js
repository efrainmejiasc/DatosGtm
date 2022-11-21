$(document).ready(function () {
    console.log("ready!");

    var date = FechaActual();
    $('#fechaNacimiento').val(date);

});


function onSubmit(token) {
    document.getElementById("demo-form").submit();
}



















function FechaActual() {

    var today = new Date();
    var year = today.getFullYear();
    var moth = (today.getMonth() + 1) <= 9 ? "0" + (today.getMonth() + 1) : (today.getMonth() + 1);
    var day = today.getDate() <= 9 ? "0" + today.getDate() : today.getDate();
    var date = year + '-' + moth + '-' + day;

    return date;

}

var whitelist = ["https://cdn.sat.gob.gt", "https://localhost:49955"];

function GetBearerValue() {
   

    var n = document.getElementById('_iframe');
    console.log(n);
  
    var remoteStorage = new CrossDomainStorage('https://cdn.sat.gob.gt', "https://localhost:49955");

    remoteStorage.requestValue("accessToken", function (key, value) {
        alert("The value for '" + key + "' is '" + value + "'");
    });
}

function CrossDomainStorage(origin, path) {
    this.origin = origin;
    this.path = path;
    this._iframe = null;
    this._iframeReady = false;
    this._queue = [];
    this._requests = {};
    this._id = 0;
}

CrossDomainStorage.prototype = {

    //restore constructor
    constructor: CrossDomainStorage,

    //public interface methods

    init: function () {

        var that = this;


            if (window.postMessage && window.JSON && window.localStorage) {
                this._iframe = document.createElement("_iframe");
                this._iframe.style.cssText = "position:absolute;width:1px;height:1px;left:-9999px;";
                document.body.appendChild(this._iframe);

                if (window.addEventListener) {
                    this._iframe.addEventListener("load", function () { that._iframeLoaded(); }, false);
                    window.addEventListener("message", function (event) { that._handleMessage(event); }, false);
                } else if (this._iframe.attachEvent) {
                    this._iframe.attachEvent("onload", function () { that._iframeLoaded(); }, false);
                    window.attachEvent("onmessage", function (event) { that._handleMessage(event); });
                }
            } else {
                throw new Error("Unsupported browser.");
            }
        

        //this._iframe.src = this.origin + this.path;

    },

    requestValue: function (key, callback) {
        var request = {
            key: key,
            id: ++this._id
        },
            data = {
                request: request,
                callback: callback
            };

        if (this._iframeReady) {
            this._sendRequest(data);
        } else {
            this._queue.push(data);
        }

        if (!this._iframe) {
            this.init();
        }
    },

    //private methods

    _sendRequest: function (data) {
        this._requests[data.request.id] = data;
        this._iframe.contentWindow.postMessage(JSON.stringify(data.request), this.origin);
    },

    _iframeLoaded: function () {
        this._iframeReady = true;

        if (this._queue.length) {
            for (var i = 0, len = this._queue.length; i < len; i++) {
                this._sendRequest(this._queue[i]);
            }
            this._queue = [];
        }
    },

    _handleMessage: function (event) {
        if (event.origin == this.origin) {
            var data = JSON.parse(event.data);
            this._requests[data.id].callback(data.key, data.value);
            delete this._requests[data.id];
        }
    }

};

