export abstract class Repository<T> {

    public static getQueryStringFromObject(options: any) {
        var params = [];
        for (var key in options)
            if (options.hasOwnProperty(key)) {
                params.push(encodeURIComponent(key) + "=" + encodeURIComponent(options[key]));
            }
        return params.join("&");
    }

    public getQueryStringFromObject(obj: any) {
        return Repository.getQueryStringFromObject(obj);
    }
}
