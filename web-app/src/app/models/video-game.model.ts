export interface VideoGame {
  id: number;
  title: string;
  genre: string;
  platform: string;
  releaseYear: number;
  price: number;
  description: string;
  imageUrl: string;
  createdAt?: string;
  updatedAt?: string | null;
}

export interface CreateVideoGameRequest {
  title: string;
  genre: string;
  platform: string;
  releaseYear: number;
  price: number;
  description: string;
  imageUrl: string;
}

export interface UpdateVideoGameRequest {
  title: string;
  genre: string;
  platform: string;
  releaseYear: number;
  price: number;
  description: string;
  imageUrl: string;
}

export interface PagedResult<T> {
  items: T[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}
