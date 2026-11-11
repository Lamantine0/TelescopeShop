(() => {
  const normalize = (value) => value.toLocaleLowerCase("ru-RU");

  document.querySelectorAll("[data-manufacturer-autocomplete]").forEach((input) => {
    const list = document.getElementById(input.getAttribute("aria-controls"));
    const source = document.getElementById(input.dataset.source);
    if (!list || !source) return;

    const clearButton = input.parentElement?.querySelector("[data-manufacturer-clear]") || null;
    const requiresInput = input.dataset.autocompleteRequiresInput === "true";

    let manufacturers = [];
    try {
      manufacturers = JSON.parse(source.textContent) || [];
    } catch {
      return;
    }

    let matches = [];
    let activeIndex = -1;

    const setExpanded = (expanded) => {
      input.setAttribute("aria-expanded", String(expanded));
      list.hidden = !expanded;
    };

    const syncClear = () => {
      if (clearButton) {
        clearButton.hidden = input.value.trim().length === 0;
      }
    };

    const close = () => {
      matches = [];
      activeIndex = -1;
      list.replaceChildren();
      input.removeAttribute("aria-activedescendant");
      setExpanded(false);
    };

    const select = (value) => {
      input.value = value;
      syncClear();
      close();
      input.focus();
    };

    const render = () => {
      list.replaceChildren();

      matches.forEach((value, index) => {
        const option = document.createElement("li");
        const optionId = `${list.id}-option-${index}`;
        option.className = "filter-suggestion";
        option.id = optionId;
        option.setAttribute("role", "option");
        option.dataset.value = value;
        option.textContent = value;

        if (index === activeIndex) {
          option.classList.add("is-active");
          option.setAttribute("aria-selected", "true");
          input.setAttribute("aria-activedescendant", optionId);
        }

        option.addEventListener("mousedown", (event) => {
          event.preventDefault();
          select(value);
        });

        list.appendChild(option);
      });

      if (activeIndex < 0) {
        input.removeAttribute("aria-activedescendant");
      }

      setExpanded(matches.length > 0);
    };

    const update = () => {
      const term = normalize(input.value.trim());

      if (requiresInput && !term) {
        close();
        return;
      }

      matches = manufacturers
        .filter((value) => {
          const normalized = normalize(value);
          if (!term) return true;

          return requiresInput
            ? normalized.startsWith(term) || normalized.includes(` ${term}`)
            : normalized.includes(term);
        })
        .slice(0, 8);
      activeIndex = matches.length > 0 ? 0 : -1;
      render();
      syncClear();
    };

    if (clearButton) {
      clearButton.addEventListener("click", () => {
        input.value = "";
        close();
        syncClear();
        input.focus();
      });
    }

    input.addEventListener("input", update);
    input.addEventListener("focus", update);
    syncClear();

    input.addEventListener("keydown", (event) => {
      if (event.key === "Escape") {
        close();
        return;
      }

      if (!matches.length) return;

      if (event.key === "ArrowDown") {
        event.preventDefault();
        activeIndex = (activeIndex + 1) % matches.length;
        render();
      } else if (event.key === "ArrowUp") {
        event.preventDefault();
        activeIndex = (activeIndex - 1 + matches.length) % matches.length;
        render();
      } else if (event.key === "Enter" && activeIndex >= 0) {
        event.preventDefault();
        select(matches[activeIndex]);
      }
    });

    document.addEventListener("mousedown", (event) => {
      if (!input.parentElement.contains(event.target)) close();
    });

    input.addEventListener("blur", () => {
      window.setTimeout(close, 100);
    });
  });
})();
