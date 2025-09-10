import {
    Component,
    Output,
    Input,
    OnInit,
    EventEmitter,
    ChangeDetectorRef,
} from '@angular/core';
import { TreeViewSettings, TreeViewSelectionMode, TreeViewItem } from '@indusoft/tree-view';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: [ './app.component.scss']
})
export class AppComponent  {

    title = 'TreeView';
    constructor( public ref: ChangeDetectorRef) {
    }

    public settingsSingle: TreeViewSettings = {
        selectionMode: TreeViewSelectionMode.Single,
        workflowSettings: {
            showFuncButtons: true,
            showHeader: true,
            showFilter: true,
            allowLabelEditor: true,
            allowDragNodes: true,
            allowFavorite: true,
            saveOnConfirm: true
        }
    };

    public data: TreeViewItem[] = [
        {
            label: 'Папка 1',
            data: { id: 1, name: 'Папка 1', value: 'Папка 1' },
            collapsedIcon: 'pi pi-folder',
            expandedIcon: 'pi pi-folder-open',
            isExpanded: true,
            children: [
                {
                    label: 'Подпапка 1.1',
                    data: { id: 11, name: 'Подпапка 1.1', value: 'Подпапка 1.1' },
                    collapsedIcon: 'fa fa-folder',
                    expandedIcon: 'fa fa-folder-open'
                },
                {
                    label: 'Подпапка 1.2',
                    data: { id: 12, name: 'Подпапка 1.2', value: 'Подпапка 1.2' },
                    collapsedIcon: 'fa fa-folder',
                    expandedIcon: 'fa fa-folder-open'
                }
            ]
        },
        {
            label: 'Пункт меню 2',
            data: { id: 2, name: 'Пункт меню 2', value: 'Пункт меню 2' },
            children: [
                {
                    label: 'Пункт меню 2.1',
                    data: { id: 21, name: 'Пункт меню 2.1', value: 'Пункт меню 2.1' },
                    collapsedIcon: 'fa fa-times',
                    expandedIcon: 'fa fa-times'
                },
                {
                    label: 'Подпапка 2.2',
                    data: { id: 22, name: 'Подпапка 2.2', value: 'Подпапка 2.2' },
                    collapsedIcon: 'fa fa-folder',
                    expandedIcon: 'fa fa-folder-open'
                }
            ]
        },
        {
            label: 'Файл 3',
            data: { id: 3, name: 'Файл 3', value: 'Файл 3' },
            collapsedIcon: 'fa fa-file',
            expandedIcon: 'fa fa-file'
        },
        {
            label: 'Папка 4',
            data: { id: 4, name: 'Папка 4', value: 'Папка 4' },
            collapsedIcon: 'fa fa-folder',
            expandedIcon: 'fa fa-folder-open',
            children: [
                {
                    label: 'Подпапка 4.1',
                    data: { id: 41, name: 'Подпапка 4.1', value: 'Подпапка 4.1' },
                    collapsedIcon: 'fa fa-folder',
                    expandedIcon: 'fa fa-folder-open'
                },
                {
                    label: 'Подпапка 4.2',
                    data: { id: 42, name: 'Подпапка 4.2', value: 'Подпапка 4.2' },
                    collapsedIcon: 'fa fa-folder',
                    expandedIcon: 'fa fa-folder-open'
                },
                {
                    label: 'Подпапка 4.3',
                    data: { id: 43, name: 'Подпапка 4.3', value: 'Подпапка 4.3' },
                    collapsedIcon: 'fa fa-folder',
                    expandedIcon: 'fa fa-folder-open'
                }
            ]
        }
    ];
  

    public treeWidth: number = 20;
    public readonly paddingInPercent = 10 / (window.innerWidth / 100);

}
