class AppBridge {
    createAppBridge(appConfig) {
        const appBridge = window['app-bridge'];
        const createApp = appBridge.default;
        const { actions } = appBridge;
        const { NavigationMenu, AppLink, Redirect } = actions;
        const app = createApp(appConfig);
        window.createdAppBridge = app;
        const templates = AppLink.create(app, {
            label: 'Templates',
            destination: '/templates',
        });
        const connectors = AppLink.create(app, {
            label: 'Connectors',
            destination: '/connectors',
        });
        const activity = AppLink.create(app, {
            label: 'Activity',
            destination: '/activity',
        });
        const subscription = AppLink.create(app, {
            label: 'Plans',
            destination: '/subscription',
        });
        const help = AppLink.create(app, {
            label: 'Help',
            destination: '/help',
        });
        const navigationMenu = NavigationMenu.create(app, {
            items: [templates, connectors, activity, subscription, help],
        });
        return app;
    }

    getAppBridge(){
        return window.createdAppBridge;
    }
    
    subscribeNavigation(navigationManager){
        const app = window.createdAppBridge;
        let Redirect = window['app-bridge'].actions.Redirect;
        app.subscribe(Redirect.Action.APP, (payload) => {
            // Do something with the redirect
            navigationManager.invokeMethodAsync("NavigateTo", payload.path);
            console.log(`Navigated to ${payload.path}`);
        });
    }

    async getNewSessionToken() {
        const app = window.createdAppBridge;
        return await window['app-bridge-utils'].getSessionToken(app);
    }
    
    redirect(url) {
        const app = window.createdAppBridge;
        let Redirect = window['app-bridge'].actions.Redirect;
        console.log(Redirect.create(app).dispatch(Redirect.Action.APP, url));
    }
}

window.appBridge = new AppBridge();