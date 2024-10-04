class AppBridge {
    createAppBridge(appConfig) {
        let createApp = window['app-bridge'].default;
        return createApp(appConfig);
    }

    async getNewSessionToken(app) {
        return await window['app-bridge-utils'].getSessionToken(app);
    }
}

window.appBridge = new AppBridge();