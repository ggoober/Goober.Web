import { Component, HostListener } from '@angular/core';
import { ContextMenuModel, ContextMenuModelArgs } from '@indusoft/context-menu';

@Component({
  selector: 'context-menu-page',
  templateUrl: './context-menu.component.html',
    styleUrls: ['./context-menu.component.scss']
})
export class AppComponent {
  title = 'context-menu-component';
  constructor(){}
  exampleArr1 = [
    {
      data: 'Правой жми',
      abc: 'abcData123',
      value: 1
    },
    {
      data: 'Правой жми',
      abc: 'abcData321',
      value: 1
    },
    {
      
      data: 'Правой жми',
      abc: 'abcData552',
      value: 3
    },
    {
      data: 'Правой жми',
      abc: 'abcData1256',
      value: 4
    }
  ]

  exampleArr2 = [
    {
      data: 'Правой жми',
      abc: 'abcData123',
      value: 1
    },
    {
      data: 'Правой жми',
      abc: 'abcData321',
      value: 1
    }
  ]

  myCallbackFunction(args: any): void {
    window.alert(args)
}
  
  public firstType: Array<ContextMenuModel> = [
    {
      menuText: "Тест Один - 1",
      menuIconClass: 'fa-plus',
      isDisabled: true,
      menuEvent: (): void => this.myCallbackFunction('Тест Один - 1'),
      children: [
        {
          menuText: 'Второй уровень - 1',
          menuEvent: (): void => this.myCallbackFunction('Второй уровень - 1')
        },
        {
          menuText: 'Второй уровень длинное название - 2',
          menuEvent: (): void => this.myCallbackFunction('Второй уровень длинное название - 2'),
        },
        {
          menuText: 'Второй уровень - 3',
          menuEvent: (): void => this.myCallbackFunction('Второй уровень - 3')
        }
      ]
    },
    {
      menuText: 'Тест Два - 2',
      menuIconClass: 'fa-external-link-alt',
      hideOnClick: false
    },
    {
      menuText: 'Тест Три - 3',
      menuIconClass: 'fa-shield-alt',
      menuEvent: (): void => this.myCallbackFunction('Тест Три - 3'),
      children: [
        {
          menuText: 'Второй уровень - 1',
          isDisabled: true,
        },
        {
          menuText: 'Второй уровень - 2',
          menuEvent: (): void => this.myCallbackFunction('Второй уровень - 2')
        },
        {
          menuText: 'Второй уровень - 3',
        },
        {
          menuText: 'Второй уровень - 4',
          children: [
            {
              menuText: 'Третий уровень длинное название',
              menuEvent: (): void => this.myCallbackFunction('Третий уровень длинное название'),
              children: [
                {
                  menuText: 'Четвертый уровень длинное название',
                  children: [
                    {
                      menuText: 'Пятый уровень',
                      menuEvent: (): void => this.myCallbackFunction('Пятый уровень'),
                    }
                  ]
                }
              ]
            }
          ]
        }
      ]
    },
  ];

  public secondType: Array<ContextMenuModel> = [
    {
      menuText: 'Тест Один - 4',
      menuEvent: (): void => this.myCallbackFunction('Тест Один - 4')
    },
    {
      menuText: 'Тест Два - 5'
    }
  ];

  private i : number = 0;

  handleMenuItemClick(event: ContextMenuModelArgs) {
    switch (event.index) {
      case 0:
           console.log(event.func, this.i++);
           break;
      case 1:
          console.log(event.func);
          break;
      case 2:
        console.log(event.func);
        break;
    }
  }


}
