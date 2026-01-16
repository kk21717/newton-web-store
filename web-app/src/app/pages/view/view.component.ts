import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { NgbModal, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { VideoGame, UpdateVideoGameRequest } from '../../models/video-game.model';
import { VideoGameService } from '../../services/video-game.service';

@Component({
  selector: 'app-view',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink, NgbModule],
  templateUrl: './view.component.html',
  styleUrl: './view.component.css'
})
export class ViewComponent implements OnInit {
  videoGame: VideoGame | null = null;
  selectedGame: VideoGame | null = null;
  loading = true;
  saving = false;
  error: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private modalService: NgbModal,
    private videoGameService: VideoGameService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.loadVideoGame(+id);
    } else {
      this.error = 'No video game ID provided.';
      this.loading = false;
    }
  }

  loadVideoGame(id: number): void {
    this.loading = true;
    this.error = null;

    this.videoGameService.getById(id).subscribe({
      next: (game) => {
        this.videoGame = game;
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Error loading video game:', err);
        this.error = 'Failed to load video game details. The item may not exist.';
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  openEditModal(content: any): void {
    if (!this.videoGame) return;
    this.selectedGame = { ...this.videoGame };
    this.modalService.open(content, { centered: true, size: 'lg' });
  }

  saveChanges(modal: any): void {
    if (!this.selectedGame) return;

    this.saving = true;
    const updateRequest: UpdateVideoGameRequest = {
      title: this.selectedGame.title,
      genre: this.selectedGame.genre,
      platform: this.selectedGame.platform,
      releaseYear: this.selectedGame.releaseYear,
      price: this.selectedGame.price,
      description: this.selectedGame.description,
      imageUrl: this.selectedGame.imageUrl
    };

    this.videoGameService.update(this.selectedGame.id, updateRequest).subscribe({
      next: (updatedGame) => {
        this.videoGame = updatedGame;
        this.saving = false;
        modal.close();
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Error updating video game:', err);
        this.saving = false;
        this.cdr.detectChanges();
        alert('Failed to update video game. Please try again.');
      }
    });
  }
}
