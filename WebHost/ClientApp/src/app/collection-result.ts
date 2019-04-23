export class CollectionResult<T>  {
    public values:T[];
    public totalCount:number;
    constructor()
    {
        this.values=new Array<T>();
        this.totalCount=0;
    }
}

