import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map, delay } from 'rxjs/operators';
import { IElement } from '@indusoft/multiselect';

@Injectable({
    providedIn: 'root'
})
export class DataService {
    private url = './assets/data.txt'; //адрес файла данных

    private options = {
        responseType: 'text' as const
    };

    constructor(private httpClient: HttpClient) { }

    // метод для чтения данных и добавления в уже имеющийся список с учетом имеющихся элементов
    public readForAdd(data_array: IElement[], count: number): Observable<void> {
        return this.httpClient.get(this.url, this.options).pipe(
            map((data) => {
                let i = 0;
                for (const line of data.split(/[\r\n]+/)) {
                    // перебор по строкам
                    let element = this.newElement(line.split('\t')); // формируем новый элемент массива из строки
                    if (!this.isIncluded(element, data_array)) {
                        // проверяем его на наличие в текущем массиве
                        data_array.push(element); // добавляем, если не нашли
                        if (++i == count) {
                            break; // заканчиваем чтение, если достигли предела счетчика
                        }
                    }
                }
            }),
            delay(100)
        );
    }

    // метод для чтения данных при поиске с учетом имеющихся
    public readForSearch(
        data_array: IElement[],
        searchTerm: string
    ): Observable<void> {
        return this.httpClient.get(this.url, this.options).pipe(
            map((data) => {
                for (const line of data.split(/[\r\n]+/)) {
                    // перебор по строкам
                    let element = this.newElement(line.split('\t')); // формируем новый элемент массива из строки
                    if (this.isFound(element, searchTerm)) {
                        // проверка считанного элемента на соответствие поиску
                        if (!this.isIncluded(element, data_array)) {
                            // если элемент соответствует поиску, проверка на наличие в текущем массиве
                            data_array.push(element); // добавляем, если не нашли
                        }
                    }
                }
            }),
            delay(100)
        );
    }

    // функция формирования объекта для массива данных
    private newElement(array: string[]): IElement {
        return {
            value: +array[0],
            title: array[1],
            groupName: array[3],
            imgSrc: array[4]
        };
    }

    // функция сравнения двух JSON-объектов
    private isIncluded(newElem: IElement, elements: IElement[]): boolean {
        for (const currentElem of elements) {
            if (newElem.value == currentElem.value) {
                return true;
            }
        }
        return false;
    }

    // функция сравнения поискового запроса с элементом
    private isFound(item: IElement, term: string): boolean {
        term = term.toLocaleLowerCase();
        return item.title.toLocaleLowerCase().includes(term);
    }
}
