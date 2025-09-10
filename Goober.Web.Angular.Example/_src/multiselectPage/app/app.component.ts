import { Component } from '@angular/core';
import {
    IElement,
    ISearchTerm,
    MultiselectSettings
} from "@indusoft/multiselect";
import { debounce } from 'debounce';
import { DataService } from '../app/data.service';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./pure.css','./app.component.scss']
})
export class AppComponent {
    title = 'Multiselect';
    private app_maxLoadedItems = 5;
    constructor(private dataService: DataService) { }
    app_loadedElements_full: IElement[] = [
        { value: 1, title: 'Элемент 1', groupName: 'Группа 1', imgSrc: 'https://via.placeholder.com/150/0000FF' },
        { value: 2, title: 'Элемент 2', groupName: 'Группа 1', imgSrc: 'https://via.placeholder.com/150/FF0000' },
        { value: 3, title: 'Элемент 3', groupName: 'Группа 2', imgSrc: 'https://via.placeholder.com/150/FFFF00' },
        { value: 4, title: 'Элемент 4', groupName: 'Группа 2', imgSrc: 'https://via.placeholder.com/150/00FF00' }
    ];

    app_loadedElements_lazy: IElement[] = [];

    public app_MSSettings1: Partial<MultiselectSettings> = {};

    public app_MSSettings2: Partial<MultiselectSettings> = {
        groupable: true
    };

    public app_MSSettings3: Partial<MultiselectSettings> = {
        useImg: true
    };

    public app_MSSettings4: Partial<MultiselectSettings> = {
        multiple: true
    };

    public app_MSSettings5: Partial<MultiselectSettings> = {
        multiple: true,
        groupable: true
    };

    public app_MSSettings6: Partial<MultiselectSettings> = {
        multiple: true,
        addable: true
    };

    public app_MSSettings7: Partial<MultiselectSettings> = {
        multiple: true,
        lazyLoading: true
    };

    public app_addNewElements(): void {
        this.app_MSSettings7.loading = true;
        this.dataService
            .readForAdd(this.app_loadedElements_lazy, this.app_maxLoadedItems)
            .subscribe(() => {
                this.app_loadedElements_lazy = this.app_loadedElements_lazy.concat();
                this.app_MSSettings7.loading = false;
            });
    }

    public app_searchForNewElements(element: ISearchTerm): void {
        this.app_MSSettings7.loading = true;
        this.app_searchForNewElements_(element.term);
    }

    private app_searchForNewElements_ = debounce((term: string) => {
        this.dataService
            .readForSearch(this.app_loadedElements_lazy, term)
            .subscribe(() => {
                this.app_loadedElements_lazy = this.app_loadedElements_lazy.concat();
                this.app_MSSettings7.loading = false;
            });
    }, 800);
}
