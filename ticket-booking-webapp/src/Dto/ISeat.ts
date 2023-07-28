export interface ISeatEvent {
  seatId: string;
  eventId: string;
  seatStatus: 0 | 1 | 2 | -1; //'default' | 'picked' | 'banned'; 'picking' is -1
  price: number;
  seat: ISeat;
}

interface ISeat {
  id: string;
  name: string;
  type: 0 | 1 | 2; // 0 is standard, 1 is vip, 2 is sweet-box
}
