import { IBasket } from './ibasket';
import { IBasketItem } from './ibasketitem';
import { v4 as uuidv4 } from 'uuid';


export class Basket implements IBasket {
    id = uuidv4();
    items: IBasketItem[] = [];
}