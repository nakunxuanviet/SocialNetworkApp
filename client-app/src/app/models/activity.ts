import { Paginated } from "./paginated";

export interface Activity {
  id: number;
  title: string;
  date: string;
  description: string;
  category: string;
  city: string;
  venue: string;
  isCancelled: boolean;
}

export interface PaginatedActivity extends Paginated {
  items: Activity[]
}
