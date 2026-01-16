import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { NgbModal, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { VideoGame, CreateVideoGameRequest, UpdateVideoGameRequest, PagedResult } from '../../models/video-game.model';
import { VideoGameService } from '../../services/video-game.service';

export type SearchType = 'all' | 'title' | 'genre' | 'platform';

@Component({
  selector: 'app-edit',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink, NgbModule],
  templateUrl: './edit.component.html',
  styleUrl: './edit.component.css'
})
export class EditComponent implements OnInit {
  videoGames: VideoGame[] = [];
  selectedGame: VideoGame | null = null;
  loading = true;
  saving = false;
  error: string | null = null;
  isEditMode = true;

  // Pagination state
  pageNumber = 1;
  pageSize = 10;
  totalCount = 0;
  totalPages = 0;
  hasPreviousPage = false;
  hasNextPage = false;

  // Search/filter state
  searchType: SearchType = 'all';
  searchTerm = '';

  constructor(
    private modalService: NgbModal,
    private videoGameService: VideoGameService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadVideoGames();
  }

  loadVideoGames(): void {
    this.loading = true;
    this.error = null;

    const request$ = this.getPagedRequest();

    request$.subscribe({
      next: (result: PagedResult<VideoGame>) => {
        this.videoGames = result.items;
        this.totalCount = result.totalCount;
        this.totalPages = result.totalPages;
        this.hasPreviousPage = result.hasPreviousPage;
        this.hasNextPage = result.hasNextPage;
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Error loading video games:', err);
        this.error = 'Failed to load video games. Please make sure the API server is running.';
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  private getPagedRequest() {
    const term = this.searchTerm.trim();

    switch (this.searchType) {
      case 'title':
        return this.videoGameService.searchPaged(term, this.pageNumber, this.pageSize);
      case 'genre':
        return this.videoGameService.getByGenrePaged(term, this.pageNumber, this.pageSize);
      case 'platform':
        return this.videoGameService.getByPlatformPaged(term, this.pageNumber, this.pageSize);
      case 'all':
      default:
        if (term) {
          return this.videoGameService.searchPaged(term, this.pageNumber, this.pageSize);
        }
        return this.videoGameService.getAllPaged(this.pageNumber, this.pageSize);
    }
  }

  onSearch(): void {
    this.pageNumber = 1;
    this.loadVideoGames();
  }

  onClearSearch(): void {
    this.searchTerm = '';
    this.searchType = 'all';
    this.pageNumber = 1;
    this.loadVideoGames();
  }

  onPageChange(page: number): void {
    if (page < 1 || page > this.totalPages) return;
    this.pageNumber = page;
    this.loadVideoGames();
  }

  onPageSizeChange(): void {
    this.pageNumber = 1;
    this.loadVideoGames();
  }

  getPageNumbers(): number[] {
    const pages: number[] = [];
    const maxVisiblePages = 5;
    let startPage = Math.max(1, this.pageNumber - Math.floor(maxVisiblePages / 2));
    let endPage = Math.min(this.totalPages, startPage + maxVisiblePages - 1);

    if (endPage - startPage + 1 < maxVisiblePages) {
      startPage = Math.max(1, endPage - maxVisiblePages + 1);
    }

    for (let i = startPage; i <= endPage; i++) {
      pages.push(i);
    }
    return pages;
  }

  openEditModal(content: any, game: VideoGame): void {
    this.isEditMode = true;
    this.selectedGame = { ...game };
    this.modalService.open(content, { centered: true, size: 'lg' });
  }

  openAddModal(content: any): void {
    this.isEditMode = false;
    this.selectedGame = {
      id: 0,
      title: '',
      genre: '',
      platform: '',
      releaseYear: new Date().getFullYear(),
      price: 0,
      description: '',
      imageUrl: ''
    };
    this.modalService.open(content, { centered: true, size: 'lg' });
  }

  saveChanges(modal: any): void {
    if (!this.selectedGame) return;

    this.saving = true;

    if (this.isEditMode) {
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
          const index = this.videoGames.findIndex(g => g.id === updatedGame.id);
          if (index !== -1) {
            this.videoGames[index] = updatedGame;
          }
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
    } else {
      const createRequest: CreateVideoGameRequest = {
        title: this.selectedGame.title,
        genre: this.selectedGame.genre,
        platform: this.selectedGame.platform,
        releaseYear: this.selectedGame.releaseYear,
        price: this.selectedGame.price,
        description: this.selectedGame.description,
        imageUrl: this.selectedGame.imageUrl
      };

      this.videoGameService.create(createRequest).subscribe({
        next: () => {
          this.saving = false;
          modal.close();
          this.loadVideoGames();
        },
        error: (err) => {
          console.error('Error creating video game:', err);
          this.saving = false;
          this.cdr.detectChanges();
          alert('Failed to create video game. Please try again.');
        }
      });
    }
  }

  deleteGame(id: number): void {
    if (!confirm('Are you sure you want to delete this video game?')) {
      return;
    }

    this.videoGameService.delete(id).subscribe({
      next: () => {
        this.videoGames = this.videoGames.filter(g => g.id !== id);
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Error deleting video game:', err);
        alert('Failed to delete video game. Please try again.');
      }
    });
  }
}
