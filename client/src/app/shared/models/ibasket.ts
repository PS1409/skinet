import { IBasketItem } from './ibasketitem';

export interface IBasket {
    id: string;
    items: IBasketItem[];
}