import { Component } from '@angular/core';
import { OverlayWindowPosition, OverlayWindowService, OverlayWindowSettings } from "@indusoft/overlay-window";
import { OverlayComponent } from './overlay/overlay.component';
import { TreeViewSettings, TreeViewSelectionMode, TreeViewItem, TreeViewItemSelectedArgs } from '@indusoft/tree-view';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./pure.css','./app.component.scss']
})
export class AppComponent {
    title = 'Form';
    public treeOpen = true;
    constructor(private overlayService: OverlayWindowService) { }
    public openOverlay(): void {
        const settings = new OverlayWindowSettings(undefined, undefined, undefined, false, true, OverlayWindowPosition.Center);
        const ref = this.overlayService.showWindow(OverlayComponent, settings);

        ref.onClose().subscribe(result => {
            console.log(result);
        })
    };

    public settingsSingle: TreeViewSettings = {
        selectionMode: TreeViewSelectionMode.Single
    }
    public dataSingle: TreeViewItem[] = [
        {
            label: 'Форма 1',
            data: { id: 1, name: 'Form 1' },
            collapsedIcon: 'pi pi-folder',
            expandedIcon: 'pi pi-folder-open',
            isExpanded: true,
            children: [
                {
                    label: 'Вкладка 1',
                    data: { id: 11, name: 'Tab 1', for: 'tab1' },
                    collapsedIcon: 'fa fa-file',
                    expandedIcon: 'fa fa-file'
                },
                {
                    label: 'Вкладка 2',
                    data: { id: 12, name: 'Tab 2', for: 'tab2' },
                    collapsedIcon: 'fa fa-file',
                    expandedIcon: 'fa fa-file'
                },
                {
                    label: 'Вкладка 3',
                    data: { id: 13, name: 'Tab 3', for: 'tab3' },
                    collapsedIcon: 'fa fa-file',
                    expandedIcon: 'fa fa-file'
                },
                {
                    label: 'Вкладка 4',
                    data: { id: 14, name: 'Tab 4', for: 'tab4' },
                    collapsedIcon: 'fa fa-file',
                    expandedIcon: 'fa fa-file'
                },
                {
                    label: 'Вкладка 5',
                    data: { id: 15, name: 'Tab 5', for: 'tab5' },
                    collapsedIcon: 'fa fa-file',
                    expandedIcon: 'fa fa-file'
                }
            ]
        },
        {
            label: 'Форма 2',
            data: { id: 2, name: 'Form 2' },
            collapsedIcon: 'pi pi-folder',
            expandedIcon: 'pi pi-folder-open',
            children: [
                {
                    label: 'Вкладка 6',
                    data: { id: 21, name: 'Tab 1', for: 'tab6' },
                    collapsedIcon: 'fa fa-file',
                    expandedIcon: 'fa fa-file'
                },
                {
                    label: 'Вкладка 7',
                    data: { id: 22, name: 'Tab 2', for: 'tab7' },
                    collapsedIcon: 'fa fa-file',
                    expandedIcon: 'fa fa-file'
                },
                {
                    label: 'Вкладка 8',
                    data: { id: 23, name: 'Tab 3', for: 'tab8' },
                    collapsedIcon: 'fa fa-file',
                    expandedIcon: 'fa fa-file'
                },
                {
                    label: 'Вкладка 9',
                    data: { id: 24, name: 'Tab 4', for: 'tab9' },
                    collapsedIcon: 'fa fa-file',
                    expandedIcon: 'fa fa-file'
                },
            ]
        }
    ];

    public dataCheckBox: TreeViewItem[] = [];

    public chekTree(el: any): void {
        for (let i = 0; i <= this.dataSingle.length; i++) {
            let tabs = this.dataSingle[i]?.children
            if (tabs) {
                for (let tab of tabs) {
                    if (tab?.data?.for === el.previousElementSibling.id) {
                        let selectItem: TreeViewItemSelectedArgs = { item: tab }
                        this.itemSelected(selectItem)
                    }
                }
            }

        }
    }

    public itemSelected(event: TreeViewItemSelectedArgs): void {
        const tab = document.getElementById(event.item.data.for) as HTMLInputElement | null;
        const tabsGroup = document.getElementsByClassName('i-tab-group');
        const treeNodes = document.getElementsByClassName('p-treenode-content');
        const treeNode = document.querySelector('[aria-label="' + event.item.label + '"]')
        if (tab) {
            if (tabsGroup) {
                for (const tabGroup of tabsGroup) {
                    tabGroup.classList.remove('active')
                }
            }
            if (treeNodes) {
                for (const node of treeNodes) {
                    node.classList.remove('active')
                }
            }
            tab.parentElement?.classList.add('active');
            treeNode?.classList.add('active')
            tab.checked = true;
        }
    }

    public clickOverlay(): void {
        const settings = new OverlayWindowSettings(undefined, undefined, undefined, false, true, OverlayWindowPosition.Center);
        const ref = this.overlayService.showWindow(OverlayComponent, settings);

        ref.onClose().subscribe(result => {
            console.log(result);
        })
    }

    public closeTree(): void {
        const tree = document.getElementById('tree')
        const scheme = document.getElementById('scheme')
        if (tree && scheme) {
            if (tree.classList.contains('active')) {
                tree.classList.remove('active')
                scheme.classList.remove('sm')
                this.treeOpen = false;
            }
            else {
                tree.classList.add('active')
                scheme.classList.add('sm')
                this.treeOpen = true;
            }
        }

    }
}
