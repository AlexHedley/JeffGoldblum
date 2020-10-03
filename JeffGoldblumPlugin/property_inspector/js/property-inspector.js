// global websocket, used to communicate from/to Stream Deck software
// as well as some info about our plugin, as sent by Stream Deck software 
var websocket = null,
    uuid = null,
    inInfo = null,
    actionInfo = {},
    settingsModel = {
        Count: 5,
        //JeffsumType: 0
        JeffsumType: "Words"
    };

function connectElgatoStreamDeckSocket(inPort, inUUID, inRegisterEvent, inInfo, inActionInfo) {
    uuid = inUUID;
    actionInfo = JSON.parse(inActionInfo);
    inInfo = JSON.parse(inInfo);
    websocket = new WebSocket('ws://localhost:' + inPort);

    //initialize values
    if (actionInfo.payload.settings.settingsModel) {
        settingsModel.Count = actionInfo.payload.settings.settingsModel.Count;
        settingsModel.JeffsumType = actionInfo.payload.settings.settingsModel.JeffsumType;
    }

    document.getElementById('txtCountValue').value = settingsModel.Count;
    //document.getElementById('jeffsumtype').value = settingsModel.JeffsumType;
    //var e = document.getElementById('jeffsumtype');
    ////var type = e.options[e.selectedIndex].value;
    //e.value = settingsModel.JeffsumType
    document.getElementById('txtJeffsumTypeValue').value = settingsModel.JeffsumType;

    websocket.onopen = function () {
        var json = { event: inRegisterEvent, uuid: inUUID };
        // register property inspector to Stream Deck
        websocket.send(JSON.stringify(json));
    };

    websocket.onmessage = function (evt) {
        // Received message from Stream Deck
        var jsonObj = JSON.parse(evt.data);
        var sdEvent = jsonObj['event'];
        switch (sdEvent) {
            case "didReceiveSettings":
                if (jsonObj.payload.settings.settingsModel.Count) {
                    settingsModel.Count = jsonObj.payload.settings.settingsModel.Count;
                    document.getElementById('txtCountValue').value = settingsModel.Count;
                }
                if (jsonObj.payload.settings.settingsModel.JeffsumType) {
                    settingsModel.JeffsumType = jsonObj.payload.settings.settingsModel.JeffsumType;
                    //document.getElementById('jeffsumtype').value = settingsModel.JeffsumType;
                    //var e = document.getElementById('jeffsumtype');
                    ////var type = e.options[e.selectedIndex].value;
                    //e.value = settingsModel.JeffsumType
                    document.getElementById('txtJeffsumTypeValue').value = settingsModel.JeffsumType;
                }
                break;
            default:
                break;
        }
    };
}

const setSettings = (value, param) => {
    if (websocket) {
        settingsModel[param] = value;
        var json = {
            "event": "setSettings",
            "context": uuid,
            "payload": {
                "settingsModel": settingsModel
            }
        };
        websocket.send(JSON.stringify(json));
    }
};

