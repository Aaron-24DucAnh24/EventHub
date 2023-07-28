import { create } from "zustand";
import { immer } from "zustand/middleware/immer";

type State = {
  emailStore: string;
};

type Actions = {
  setEmailStore: (emailStore: string) => void;
};

export const useEmailStore = create(
  immer<State & Actions>((set) => ({
    emailStore: "",
    setEmailStore: (emailStore: string) =>
      set((state) => {
        state.emailStore = emailStore;
      }),
  }))
);
