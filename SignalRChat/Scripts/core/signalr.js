(function (v) {
    function isArray(o) {
        return o instanceof Object && Object.prototype.toString.call(o) === '[object Array]';
    }
    function isObject(o) {
        return o instanceof Object && Object.prototype.toString.call(o) !== '[object Array]';
    }
    function signalR(hubUrl) {
        var querys = {},
            hubs = [];
        if ($.connection) {
            if (hubUrl) $.connection.hub.url = hubUrl;

            this.connect = function (hubs) {
                if (isArray(hubs))
                    for (var i = 0; i < hubs.length; i++) init(hubs[i]);
                else if (isObject(hubs)) init(hubs);

                start();
            }

            function init(hub) {
                if (hub.query) $.extend(querys, hub.query);
                //define client depency
                var ws = $.connection[hub.name];
                if (typeof hub.client === 'undefined')
                    ws.client.receive = function (msg) { console.log('core::receive::', msg); }
                else
                    $.extend(ws.client, hub.client);
                hub.instance = ws;
                //add to hubs manager
                hubs.push(hub);
            }
            function start() {
                $.connection.hub.qs = querys;
                $.connection.hub.start().done(function () {
                    var hub;
                    for (var i = 0; i < hubs.length; i++) {
                        hub = hubs[i];
                        if (typeof hub.server === 'function') hub.server(hub.instance);
                    }
                });
            }
        }
    }

    v.signalR = signalR;
})(window)