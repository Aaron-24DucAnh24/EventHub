import { create } from "zustand";
import { immer } from "zustand/middleware/immer";

type State = {
  searchString: string;
};

type Actions = {
  setSearchString: (searchString: string) => void;
};

export const useSearchStore = create(
  immer<State & Actions>((set) => ({
    searchString: "",
    setSearchString: (searchString: string) =>
      set((state) => {
        state.searchString = searchString;
      }),
  }))
);
