"use client";
// This hook is used to debounce a value, which means that it will only update the debounced value after a certain amount of time has passed since the last change. This is useful for performance optimization, especially when dealing with user input or API calls that should not be triggered on every keystroke.
import { useEffect, useState } from "react";

export function useDebouncedValue<T>(value: T, delayMs = 400): T {
  const [debouncedValue, setDebouncedValue] = useState(value);

  useEffect(() => {
    const timeoutId = window.setTimeout(() => {
      setDebouncedValue(value);
    }, delayMs);

    return () => window.clearTimeout(timeoutId);
  }, [value, delayMs]);

  return debouncedValue;
}
