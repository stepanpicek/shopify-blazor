class AppBridge {
    createAppBridge(appConfig) {
        let createApp = window['app-bridge'].default;
        return createApp(appConfig);
    }

    async getNewSessionToken(app) {
        return await window['app-bridge-utils'].getSessionToken(app);
    }
    
    redirect(app, url) {
        let Redirect = window['app-bridge'].actions.Redirect;
        return Redirect.create(app).dispatch(Redirect.Action.APP, url);
    }
}

window.appBridge = new AppBridge();