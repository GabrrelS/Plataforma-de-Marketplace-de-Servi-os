import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Version, VersionInfo } from '../../../core/services/version';

@Component({
  selector: 'app-footer',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './footer.html',
  styleUrl: './footer.css',
})
export class Footer implements OnInit {
  private versionService = inject(Version);

  versionInfo?: VersionInfo;

  ngOnInit(): void {
    this.versionService.getVersion().subscribe({
      next: (data) => {
        this.versionInfo = data;
      },
      error: (err) => {
        console.error(err);
      },
    });
  }
}