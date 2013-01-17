declare function WebForm_GetElementById(id: string): HTMLElement;
declare function WebForm_GetElementPosition(element: HTMLElement): WebForm_Position;
declare function WebForm_GetElementDir(element: HTMLElement): string;
declare function WebForm_AppendToClassName(element: HTMLElement, cssClass: string);
declare function WebForm_RemoveClassName(element: HTMLElement, cssClass: string);
declare function WebForm_InitCallback();
declare var __theFormPostData;
declare var __theFormPostCollection;

interface WebForm_Position {
    x: number;
    y: number;
    width: number;
    height: number;
}