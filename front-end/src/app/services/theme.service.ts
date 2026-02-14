import { Injectable } from '@angular/core';

export type ThemeMode = 'light' | 'dark';

@Injectable({ providedIn: 'root' })
export class ThemeService {
  private storageKey = 'crm-theme';
  private mode: ThemeMode = 'light';

  initTheme() {
    const saved = localStorage.getItem(this.storageKey) as ThemeMode | null;

    if (saved === 'light' || saved === 'dark') {
      this.setMode(saved, false);
      return;
    }

    const prefersDark = window.matchMedia?.('(prefers-color-scheme: dark)')?.matches ?? false;
    this.setMode(prefersDark ? 'dark' : 'light', false);
  }

  getMode(): ThemeMode {
    return this.mode;
  }

  // ✅ ADD THIS (fixes your TS2339 error)
  isDark(): boolean {
    return this.mode === 'dark';
  }

  toggle() {
    this.setMode(this.mode === 'dark' ? 'light' : 'dark');
  }

  setMode(mode: ThemeMode, persist: boolean = true) {
    this.mode = mode;

    const root = document.documentElement; // <html>
    if (mode === 'dark') root.classList.add('dark');
    else root.classList.remove('dark');

    if (persist) localStorage.setItem(this.storageKey, mode);
  }
}
