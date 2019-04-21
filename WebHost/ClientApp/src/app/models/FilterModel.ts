import { Set } from './Set';

export class FilterModel {
    public sort: string;
    public order: string;
    public skip: number;
    public take: number;
    public keywords: string;
    public context: string;
    public set: Set;
}
