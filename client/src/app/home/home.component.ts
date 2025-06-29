// import { Component} from '@angular/core';
// import { RegisterComponent } from '../register/register.component';

// @Component({
//   selector: 'app-home',
//   imports: [RegisterComponent],
//   templateUrl: './home.component.html',
//   styleUrl: './home.component.css'
// })
// export class HomeComponent{

//   registerMode=false;


  
//   registerToggle()
//   {
//     this.registerMode=!this.registerMode;
//   }
//   cancelRegisterMode(event:boolean)
//   {
//     this.registerMode=event;
//   }
  
// }
import {
  Component,
  OnInit,
  Renderer2,
  ElementRef,
  ViewChild,
  AfterViewInit
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegisterComponent } from '../register/register.component';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, RegisterComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit, AfterViewInit {
  registerMode = false;
  fullText = 'Find Your Match!';
  animatedTextArray: string[] = [];
  @ViewChild('animatedContainer') animatedContainer!: ElementRef;

  constructor(private renderer: Renderer2) {}

  ngOnInit(): void {
    this.loopAnimation();
  }

  ngAfterViewInit(): void {
    this.applyRandomStyles(); // Apply initial random scatter positions
  }

  registerToggle() {
    this.registerMode = !this.registerMode;
  }

  cancelRegisterMode(event: boolean) {
    this.registerMode = event;
  }

  loopAnimation() {
    this.animateTextIn();

    setInterval(() => {
      this.animateTextIn();

      setTimeout(() => {
        this.animateTextOut();
      }, 10000); // remove after 10s
    }, 20000); // loop every 20s
  }

  animateTextIn() {
  this.animatedTextArray = new Array(this.fullText.length).fill(''); // Reset with empty slots
  const chars = this.fullText.split('');

  chars.forEach((char, i) => {
    setTimeout(() => {
      this.animatedTextArray[i] = char;
    }, i * (10000 / chars.length)); // Smooth 10s entry
  });
}

animateTextOut() {
  const len = this.animatedTextArray.length;

  for (let i = 0; i < len; i++) {
    setTimeout(() => {
      this.animatedTextArray[i] = ''; // Instead of pop, clear individual chars
    }, i * (10000 / len)); // Smooth 10s exit
  }
}


  applyRandomStyles() {
    const spans = this.animatedContainer?.nativeElement?.querySelectorAll('span');
    spans?.forEach((span: HTMLElement) => {
      const x = Math.random() * 400 - 200 + 'px';
      const y = Math.random() * 200 - 100 + 'px';
      this.renderer.setStyle(span, 'opacity', '0');
      this.renderer.setStyle(span, 'transform', `translate(${x}, ${y}) rotate(-360deg)`);
      this.renderer.setStyle(span, 'transition', 'none');
      setTimeout(() => {
        this.renderer.setStyle(span, 'transform', 'translate(0, 0) rotate(0)');
        this.renderer.setStyle(span, 'opacity', '1');
        this.renderer.setStyle(span, 'transition', 'transform 1s ease, opacity 1s ease');
      }, 100);
    });
  }
}


