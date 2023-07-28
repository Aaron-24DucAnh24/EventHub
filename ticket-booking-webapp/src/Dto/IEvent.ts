import { ICategory } from "./ICategory";
import { ISeatEvent } from "./ISeat";

export interface IEvent {
  id: string;
  image: string;
  title: string;
  minPrice: number;
  date: string;
  location: string;
  city: string;
  status: number;
  stageName: string;
  isPublished: boolean;
  duration: string;
  categories: ICategory[];
  seatEvents: ISeatEvent[];
}

export interface IEventBooking {
  eventId: string;
  image: string;
  title: string;
  date: string;
  location: string;
  stageName: string;
  duration: string;
  seats: ISeatBooking[];
}

export interface ISeatBooking {
  name: string;
  type: number;
  price: number;
}
