namespace HandHistories.Objects.Cards
{
    /// <summary>
    /// What the rest of the table saw a player do with their hole cards.
    /// Orthogonal to whether the parser has the cards (<see cref="Players.Player.HoleCards"/>)
    /// and to whether the player is the hero (<see cref="Hand.HandHistory.Hero"/>).
    /// <para>
    /// Reveal values (<see cref="ShownAtShowdown"/>, <see cref="ShownVoluntarily"/>) do not
    /// guarantee that *all* hole cards were exposed. In Omaha variants (4/5/6 hole cards) and
    /// hi/lo split games, a player may reveal only a subset. Compare
    /// <c>Player.HoleCards.Count</c> against the expected count for
    /// <c>GameDescription.GameType</c> to detect a partial reveal.
    /// </para>
    /// </summary>
    public enum RevealAction
    {
        /// <summary>Cards never shown to the table (folded early, or silent muck).</summary>
        NotShown = 0,

        /// <summary>
        /// Player reached showdown and explicitly mucked. Cards may still leak to
        /// the parser via summary lines on some sites, but the table didn't see them.
        /// </summary>
        MuckedAtShowdown = 1,

        /// <summary>
        /// Revealed at showdown — hand reached the showdown phase.
        /// Reveal may be partial (see type-level remarks).
        /// </summary>
        ShownAtShowdown = 2,

        /// <summary>
        /// Revealed voluntarily after winning uncontested (winner flashed).
        /// Reveal may be partial (see type-level remarks).
        /// </summary>
        ShownVoluntarily = 3,
    }
}
