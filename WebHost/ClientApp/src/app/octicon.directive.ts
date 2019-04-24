import { Directive, Input, OnInit, ElementRef, Renderer2 } from '@angular/core';

import * as octicons from 'octicons';

@Directive({
  selector: '[octicon]'
})

export class OcticonDirective implements OnInit {

  @Input() octicon: string;
  @Input() width: number;

  constructor(private elementRef: ElementRef, private renderer: Renderer2) { }

  ngOnInit(): void {
    const el: HTMLElement = this.elementRef.nativeElement;
    el.innerHTML = octicons[this.octicon].toSVG();

    const icon: Node = el.firstChild;
   

    if (!this.width) this.width = 26;

    this.renderer.setStyle(icon, 'width', this.width);
    this.renderer.setStyle(icon, 'height', '100%');


  }

}