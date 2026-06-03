import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VersionService, VersionInfo } from '../../../core/services/version';

@Component({
  selector: 'app-footer',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './footer.html',
  styleUrl: './footer.css'
})
export class FooterComponent implements OnInit {

  private versionService = inject(VersionService);

  info?: VersionInfo;

  ngOnInit(): void {
    this.versionService.getVersion().subscribe({
      next: (response) => {
        this.info = response;
      },
      error: (err) => {
        console.error(err);
      }
    });
  }
}