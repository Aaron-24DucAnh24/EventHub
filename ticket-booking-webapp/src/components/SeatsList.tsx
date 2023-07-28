import { ISeatEvent } from "../Dto/ISeat";
import Seat from "./Seat";

interface Props {
  seatsList: ISeatEvent[];
  setSeatsList: (seatsList: ISeatEvent[]) => void;
  pickingSeatsCount: number;
  setPickingSeatsCount: (pickingSeatsCount: number) => void;
}

const SeatsList = ({
  seatsList,
  setSeatsList,
  pickingSeatsCount,
  setPickingSeatsCount,
}: Props) => {
  return (
    <div className="flex gap-[20px] flex-wrap justify-center self-start">
      {seatsList.map((seat, seatIndex) => (
        <Seat
          key={seat.seatId}
          seatId={seat.seatId}
          typeSeat={seat.seat.type}
          seatStatus={seat.seatStatus}
          seatsList={seatsList}
          setSeatsList={setSeatsList}
          pickingSeatsCount={pickingSeatsCount}
          setPickingSeatsCount={setPickingSeatsCount}
        />
      ))}
    </div>
  );
};

export default SeatsList;
