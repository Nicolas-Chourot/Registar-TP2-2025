﻿let EndSessionAction = '/Accounts/Login';
class AutoRefreshedPanel {
    constructor(panelId, contentServiceURL, refreshRate, postRefreshCallback = null) {
        this.contentServiceURL = contentServiceURL;
        this.panelId = panelId;
        this.postRefreshCallback = postRefreshCallback;
        this.refreshRate = refreshRate * 1000;
        this.paused = false;
        this.refresh(true);
        setInterval(() => { this.refresh() }, this.refreshRate);
    }
    pause() { this.paused = true }
    restart() { this.paused = false }
    replaceContent(htmlContent) {
        if (htmlContent !== "") {
            $("#" + this.panelId).html(htmlContent);
            if (this.postRefreshCallback != null) this.postRefreshCallback();
        }
    }
    refresh(forced = false) {
        if (!this.paused) {
            $.ajax({
                url: this.contentServiceURL + (forced ? (this.contentServiceURL.indexOf("?") > -1 ? "&" : "?") + "forceRefresh=true" : ""),
                dataType: "html",
                success: (htmlContent) => {
                    if (htmlContent != "blocked")
                        this.replaceContent(htmlContent)
                },
                statusCode: {
                    401: function () {
                        debugger
                        if (EndSessionAction != "")
                            window.location = EndSessionAction + "?message=Votre compte a été bloqué!&success=false";
                        else
                            alert("Illegal access!");
                    }
                }
            })
        }
    }
    command(url, moreCallBack = null) {
        $.ajax({
            url: url,
            method: 'GET',
            success: (params) => {
                this.refresh(true);
                if (moreCallBack != null)
                    moreCallBack(params);

            }
        });
    }

    confirmedCommand(message, url, moreCallBack = null) {
        bootbox.confirm(message, (result) => { if (result) this.command(url, moreCallBack) });
    }
}
