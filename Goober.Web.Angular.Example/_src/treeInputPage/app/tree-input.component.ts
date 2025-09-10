import { ChangeDetectorRef, Component } from '@angular/core';

import { TreeInputItem, TreeInputSelectionMode } from '@indusoft/tree-input';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'tree-input-page',
  templateUrl: './tree-input.component.html',
  styleUrls: ['./tree-input.component.scss']
})
export class AppComponent {
  public disable: boolean = false;
  public readonly: boolean = false;
  public selectedByKeysItems: string = "";
  public selectedByKeyItem: string = "";
  public selectedByKeyItemsSubscription: BehaviorSubject<string> = new BehaviorSubject('111');

  constructor(public ref: ChangeDetectorRef){}

  public multipleSelectionMode: TreeInputSelectionMode = TreeInputSelectionMode.Multiple;
  public checkBoxSelectionMode: TreeInputSelectionMode = TreeInputSelectionMode.CheckBox;
  public setKey(): void {
    this.selectedByKeyItemsSubscription.next(this.selectedItemByKey)
  }
  public items: TreeInputItem[] = [
    {
      key: "1",
      label: 'Item 1',
      data: { id: 1, name: 'test name 1', value: 'test value 1' },
      collapsedIcon: 'fa fa-folder',
      expandedIcon: 'fa fa-folder-open',
      isExpanded: true,
      children: [
        {
          key: "11",
          label: 'Item 1.1',
          data: { id: 11, name: 'test name 1.1', value: 'test value 1.1' },
          collapsedIcon: 'fa fa-folder',
          expandedIcon: 'fa fa-folder-open',
          parent: {
            label: 'Item 1',
            data: { id: 1, name: 'test name 1', value: 'test value 1' },
            collapsedIcon: 'fa fa-folder',
            expandedIcon: 'fa fa-folder-open',
            isExpanded: true
          },
          children: [
            {
              key: "111",
              label: 'Item 1.1.1',
            }
          ]
        },
        {
          key: "12",
          label: 'Item 1.2',
          data: { id: 12, name: 'test name 1.2', value: 'test value 1.2' },
          collapsedIcon: 'fa fa-folder',
          expandedIcon: 'fa fa-folder-open',
          parent: {
            label: 'Item 1',
            data: { id: 1, name: 'test name 1', value: 'test value 1' },
            collapsedIcon: 'fa fa-folder',
            expandedIcon: 'fa fa-folder-open',
            isExpanded: true
          }
        },
      ]
    },
    {
      key: "2",
      label: 'Item 2',
      data: { id: 2, name: 'test name 2', value: 'test value 2' },
      children: [

      ]
    },
    {
      key: "3",
      label: 'Item 3',
      data: { id: 3, name: 'test name 2', value: 'test value 3' },
      children: [
        {
          key: "31",
          label: 'Item 3.1',
          data: { id: 31, name: 'test name 2.1', value: 'test value 3.1' },
          collapsedIcon: 'fa fa-times',
          expandedIcon: 'fa fa-times',
          parent: {
            label: 'test item 3',
            data: { id: 3, name: 'test name 2', value: 'test value 3' }
          }
        },
        {
          key: "32",
          label: 'Item 3.2',
          data: { id: 32, name: 'test name 2.2', value: 'test value 3.2' },
          collapsedIcon: 'fa fa-folder',
          expandedIcon: 'fa fa-folder-open',
          parent: {
            label: 'Item 3',
            data: { id: 3, name: 'test name 2', value: 'test value 3' }
          }
        },
      ]
    },
    {
      key: "4",
      label: 'Item 4',
      data: { id: 4, name: 'test name 3', value: 'test value 4' },
      collapsedIcon: 'fa fa-folder',
      expandedIcon: 'fa fa-folder-open',
      children: [
        {
          key: "41",
          label: 'Item 4.1',
          data: { id: 41, name: 'test name 3.1', value: 'test value 4.1' },
          collapsedIcon: 'fa fa-folder',
          expandedIcon: 'fa fa-folder-open',
          parent: {
            label: 'Item 4',
            data: { id: 4, name: 'test name 3', value: 'test value 4' },
            collapsedIcon: 'fa fa-folder',
            expandedIcon: 'fa fa-folder-open'
          }
        },
        {
          key: "42",
          label: 'Item 4.2',
          data: { id: 42, name: 'test name 4.2', value: 'test value 4.2' },
          collapsedIcon: 'fa fa-folder',
          expandedIcon: 'fa fa-folder-open',
          parent: {
            label: 'Item 4',
            data: { id: 4, name: 'test name 3', value: 'test value 4' },
            collapsedIcon: 'fa fa-folder',
            expandedIcon: 'fa fa-folder-open'
          }
        },
        {
          key: "43",
          label: 'Item 4.3',
          data: { id: 43, name: 'test name 3.3', value: 'test value 4.3' },
          collapsedIcon: 'fa fa-folder',
          expandedIcon: 'fa fa-folder-open',
          parent: {
            label: 'Item 4',
            data: { id: 4, name: 'test name 3', value: 'test value 4' },
            collapsedIcon: 'fa fa-folder',
            expandedIcon: 'fa fa-folder-open'
          }
        }
      ]
    },
  ];

  public selectedItem: TreeInputItem = {
    key: "11",
    label: 'Item 1.1',
    data: { id: 11, name: 'test name 1.1', value: 'test value 1.1' },
    collapsedIcon: 'fa fa-folder',
    expandedIcon: 'fa fa-folder-open',
    parent: {
      label: 'Item 1',
      data: { id: 1, name: 'test name 1', value: 'test value 1' },
      collapsedIcon: 'fa fa-folder',
      expandedIcon: 'fa fa-folder-open',
      isExpanded: true
    },
  };

  public selectedItems: TreeInputItem[] = [
    {
      key: "11",
      label: 'Item 1.1',
      data: { id: 11, name: 'test name 1.1', value: 'test value 1.1' },
      collapsedIcon: 'fa fa-folder',
      expandedIcon: 'fa fa-folder-open',
      parent: {
        label: 'Item 1',
        data: { id: 1, name: 'test name 1', value: 'test value 1' },
        collapsedIcon: 'fa fa-folder',
        expandedIcon: 'fa fa-folder-open',
        isExpanded: true
      },
    },
    {
      key: "2",
      label: 'Item 2',
      data: { id: 2, name: 'test name 2', value: 'test value 2' },
      children: [

      ]
    }
  ];

  public selectedItemByKey: string = "111";
  public selectedItemsByKeys: string[] = ["2", "111", "3"];

  public onItemSelectedSingle(event: any): void {
    console.log('onItemSelectedSingle ');
    console.log(event);
  }

  public onItemSelectedMultiple(event: any): void {
    console.log('onItemSelectedMultiple ');
    console.log(event);
  }

  public onItemSelectedCheckBox(event: any): void {
    console.log('onItemSelectedCheckBox ');
    console.log(event);
  }

  public toggleDisable(): void {
    this.disable = !this.disable;
  }

  public toggleReadonly(): void {
    this.readonly = !this.readonly;
  }

  public onItemsSelectedByKeys(event: TreeInputItem[]) {
    this.selectedByKeysItems = event.map(i => i.label).join('; ');
  }

  public onItemSelectedByKey(event: TreeInputItem[]) {
    this.selectedByKeyItem = event[0]?.label ?? "";
  }
}
