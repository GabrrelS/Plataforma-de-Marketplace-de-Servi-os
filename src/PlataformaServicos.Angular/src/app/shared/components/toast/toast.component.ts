import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToastService } from '../../../core/services/toast.service';

@Component({
  selector: 'app-toast',
  standalone: true,
  imports: [CommonModule],
  template: `
    @if (toastSvc.toast()) {
      <div class="toast" [class]="toastSvc.toast()!.type">
        {{ toastSvc.toast()!.message }}
      </div>
    }
  `,
  styles: [`
    .toast {
      position: fixed;
      bottom: 1.5rem;
      right: 1.5rem;
      padding: 12px 20px;
      border-radius: 8px;
      font-size: 13px;
      font-weight: 500;
      z-index: 999;
      animation: slideIn 0.2s ease;
    }
    .success { background: #16a34a; color: #fff; }
    .error   { background: #dc2626; color: #fff; }
    .info    { background: #2563eb; color: #fff; }
    @keyframes slideIn {
      from { opacity: 0; transform: translateY(8px); }
      to   { opacity: 1; transform: translateY(0); }
    }
  `]
})
export class ToastComponent {
  toastSvc = inject(ToastService);
}
