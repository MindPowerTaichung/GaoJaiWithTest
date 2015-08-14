function getQueryStringByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}

function getObjectIndexOfArray(key, value, items) {
    var return_value = -1;
    for (var i = 0; i < items.length; i++) {
        if ((key in items[i]) && items[i][key] == value) {
            return_value= i;
            break;
        }
    }
    return return_value;
}

//http://www.telerik.com/forums/cannot-add-a-custom-http-header
(function ($, kendo) {
    "use strict";
    kendo.MPAuthorization = kendo.MP || {};
    kendo.MPAuthorization.DataSource = kendo.data.DataSource.extend({
        init: function (options) {
            if (options.transport && options.transport.read) {
                options.transport.read.beforeSend = function (xhr) {
                    xhr.setRequestHeader('Authorization', 'Basic ' + Cookies.get('token'));
                };
            }
            if (options.transport && options.transport.update) {
                options.transport.update.beforeSend = function (xhr) {
                    xhr.setRequestHeader('Authorization', 'Basic ' + Cookies.get('token'));
                };
            }
            if (options.transport && options.transport.destroy) {
                options.transport.destroy.beforeSend = function (xhr) {
                    xhr.setRequestHeader('Authorization', 'Basic ' + Cookies.get('token'));
                };
            }
            if (options.transport && options.transport.create) {
                options.transport.create.beforeSend = function (xhr) {
                    xhr.setRequestHeader('Authorization', 'Basic ' + Cookies.get('token'));
                };
            }
            kendo.data.DataSource.fn.init.call(this, options);
        }
    });
})($, kendo);