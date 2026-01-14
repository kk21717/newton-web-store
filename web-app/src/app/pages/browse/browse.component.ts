import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { VideoGame } from '../../models/video-game.model';

@Component({
  selector: 'app-browse',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './browse.component.html',
  styleUrl: './browse.component.css'
})
export class BrowseComponent {
  videoGames: VideoGame[] = [
    {
      id: 1,
      title: 'The Legend of Zelda: Tears of the Kingdom',
      genre: 'Action-Adventure',
      platform: 'Nintendo Switch',
      releaseYear: 2023,
      price: 69.99,
      description: 'An epic adventure awaits in this sequel to Breath of the Wild.',
      imageUrl: 'https://placehold.co/400x300/1a472a/ffffff?text=Zelda+TOTK'
    },
    {
      id: 2,
      title: 'God of War Ragnarök',
      genre: 'Action-Adventure',
      platform: 'PlayStation 5',
      releaseYear: 2022,
      price: 59.99,
      description: 'Kratos and Atreus embark on an epic journey through the Nine Realms.',
      imageUrl: 'https://placehold.co/400x300/2d3436/ffffff?text=God+of+War'
    },
    {
      id: 3,
      title: 'Elden Ring',
      genre: 'Action RPG',
      platform: 'Multi-platform',
      releaseYear: 2022,
      price: 59.99,
      description: 'A dark fantasy action RPG created by FromSoftware and George R.R. Martin.',
      imageUrl: 'https://placehold.co/400x300/4a4a4a/ffffff?text=Elden+Ring'
    },
    {
      id: 4,
      title: 'Hogwarts Legacy',
      genre: 'Action RPG',
      platform: 'Multi-platform',
      releaseYear: 2023,
      price: 59.99,
      description: 'Experience the wizarding world in this open-world action RPG.',
      imageUrl: 'https://placehold.co/400x300/5d4e37/ffffff?text=Hogwarts'
    },
    {
      id: 5,
      title: 'Spider-Man 2',
      genre: 'Action-Adventure',
      platform: 'PlayStation 5',
      releaseYear: 2023,
      price: 69.99,
      description: 'Swing through New York as both Peter Parker and Miles Morales.',
      imageUrl: 'https://placehold.co/400x300/c0392b/ffffff?text=Spider-Man+2'
    },
    {
      id: 6,
      title: 'Starfield',
      genre: 'Action RPG',
      platform: 'Xbox/PC',
      releaseYear: 2023,
      price: 69.99,
      description: 'Explore the vastness of space in Bethesda\'s new sci-fi RPG.',
      imageUrl: 'https://placehold.co/400x300/1e3799/ffffff?text=Starfield'
    }
  ];
}
